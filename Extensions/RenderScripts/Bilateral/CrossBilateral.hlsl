// This file is a part of MPDN Extensions.
// https://github.com/zachsaw/MPDN_Extensions
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

// -- Misc --
sampler s0 : register(s0);
sampler sUV : register(s1);

float4 p0     : register(c0);
float2 p1     : register(c1);
float4 size1  : register(c2);
float4 args0  : register(c3);

#define width  (p0[0])
#define height (p0[1])
#define chromaSize size1

#define dxdy (p1.xy)
#define ddxddy (chromaSize.zw)
#define chromaOffset (args0.xy)
#define radius 0.66

#define noise 1.0/(sqrt(12.0)*255.0)

// -- Window Size --
#define taps 4
#define even (taps - 2 * (taps / 2) == 0)
#define minX (1-ceil(taps/2.0))
#define maxX (floor(taps/2.0))

#define factor (ddxddy/dxdy)
#define Kernel(x) saturate((taps - abs(x)) * factor)

// -- Convenience --
#define sqr(x) dot(x,x)

// -- Input processing --
// Chroma value
#define Get(x,y)     tex2D(sUV,ddxddy*(pos+int2(x,y)+0.5))

// -- Colour space Processing --
#define Kb args0[2]
#define Kr args0[3]
#include "../Common/ColourProcessing.hlsl"

// -- Main Code --
float4 main(float2 tex : TEXCOORD0) : COLOR{
    float4 c0 = tex2D(s0, tex);
    float y = c0.x;

    // Calculate position
    float2 pos = tex * chromaSize.xy - chromaOffset - 0.5;
    float2 offset = pos - (even ? floor(pos) : round(pos));
    pos -= offset;

    float3 mean = 0;
    float3 mean2 = 0;
    float2 meanYUV = 0;
    float weightSum = 0;
    float weightSum2 = 0;
    [loop] for (int X = minX; X <= maxX; X++)
    [loop] for (int Y = minX; Y <= maxX; Y++)
    {
        float dI2 = sqr(Get(X,Y).x - c0.x);
        float dXY2 = sqr((float2(X,Y) - offset)/radius);

        // float2 kernel = 1-smoothstep(0,taps/2.0,abs(float2(X,Y) - offset)); // Kernel(float2(X,Y) - offset);
        // float weight = kernel.x*kernel.y * rsqrt(dI2 + sqr(noise));
        float weight = exp(-0.5*dXY2)*rsqrt(dI2 + sqr(noise));
        
        mean += weight*Get(X,Y);
        mean2 += weight*Get(X,Y)*Get(X,Y);
        meanYUV += weight*Get(X,Y).x*Get(X,Y).yz;
        weightSum += weight;
        weightSum2 += weight*weight;
    }
    mean /= weightSum;
    float3 Var = (mean2 / weightSum) - mean*mean;
    float2 Cov = (meanYUV / weightSum) - mean.x*mean.yz;

    // Var += sqr(1.0/255.0)/12.0;
    Var.yz += sqr(0.01);
    Var.x  += sqr(noise);

    float n = weightSum * sqrt(0.5 * (Var.x + sqr(mean.x - c0.x)) + sqr(noise));
    float2 R2 = saturate(Cov*Cov / (Var.x * Var.yz));
    float2 err = (1 - R2) * (1/n + sqr((c0 - mean).x) / Var.x) / n;
    
    // balance systematic error vs accuracy
    float2 a = 0.5;
    float strength = a * R2 * n / lerp(1 - R2, R2 * n, a);

    // return float4(strength, 0.5, 0.5,  0);

    // Update c0
    c0.yz = mean.yz + strength * (c0 - mean).x * Cov / Var.x;

    return c0;
}