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
// 

using System;
using DirectShowLib;
using Mpdn.Extensions.Framework;

namespace Mpdn.Examples.AudioScripts
{
    public class Gargle : Extensions.Framework.AudioScript
    {
        private const int GARGLE_RATE = 10;
        private const int SHAPE = 1; // 0=Triangle, 1=Sqaure

        private short m_Channels;
        private int m_SamplesPerSec;
        private int m_BytesPerSample;

        private int m_Phase;

        public override ExtensionUiDescriptor Descriptor
        {
            get
            {
                return new ExtensionUiDescriptor
                {
                    Guid = new Guid("A27971B2-F625-4AC8-9AC5-5B448AB77BB6"),
                    Name = "Gargle",
                    Description = "Simple audio gargle example"
                };
            }
        }

        public override void OnGetMediaType(WaveFormatExtensible format)
        {
            m_Channels = format.nChannels;
            m_SamplesPerSec = format.nSamplesPerSec;
            m_BytesPerSample = format.wBitsPerSample/8;
        }

        public override bool Process()
        {
            AudioHelpers.CopySample(Audio.Input, Audio.Output); // Gargle from input to output

            IntPtr samples;
            Audio.Output.GetPointer(out samples);
            GargleSamples(samples, Audio.Output.GetActualDataLength(), SHAPE == 0); // gargle output

            return true; // true = we handled the audio processing
        }

        private unsafe void GargleSamples(IntPtr samples, int cb, bool triangle)
        {
            var pb = (byte*) samples;

            // We know how many samples per sec and how
            // many channels so we can calculate the modulation period in samples.
            //
            int period = (m_SamplesPerSec*m_Channels)/GARGLE_RATE;

            while (cb > 0)
            {
                --cb;

                // If m_Shape is 0 (triangle) then we multiply by a triangular waveform
                // that runs 0..Period/2..0..Period/2..0... else by a square one that
                // is either 0 or Period/2 (same maximum as the triangle) or zero.
                //
                {
                    // m_Phase is the number of samples from the start of the period.
                    // We keep this running from one call to the next,
                    // but if the period changes so as to make this more
                    // than Period then we reset to 0 with a bang.  This may cause
                    // an audible click or pop (but, hey! it's only a sample!)
                    //
                    ++m_Phase;

                    if (m_Phase > period)
                        m_Phase = 0;

                    int m = m_Phase; // m is what we modulate with

                    if (triangle)
                    {
                        // Triangle
                        if (m > period/2) m = period - m; // handle downslope
                    }
                    else
                    {
                        // Square wave
                        if (m <= period/2) m = period/2;
                        else m = 0;
                    }

                    if (m_BytesPerSample == 1)
                    {
                        // 8 bit sound uses 0..255 representing -128..127
                        // Any overflow, even by 1, would sound very bad.
                        // so we clip paranoically after modulating.
                        // I think it should never clip by more than 1
                        //
                        int i = *pb - 128; // sound sample, zero based

                        i = (i*m*2)/period; // modulate
                        if (i > 127) i = 127; // clip
                        if (i < -128) i = -128;

                        *pb = (byte) (i + 128); // reset zero offset to 128

                    }
                    else if (m_BytesPerSample == 2)
                    {
                        // 16 bit sound uses 16 bits properly (0 means 0)
                        // We still clip paranoically
                        //
                        var psi = (short*) pb;
                        int i = *psi; // in a register, we might hope

                        i = (i*m*2)/period; // modulate
                        if (i > 32767) i = 32767; // clip
                        if (i < -32768) i = -32768;

                        *psi = (short) i;
                        ++pb; // nudge it on another 8 bits here to get a 16 bit step
                        --cb; // and nudge the count too.

                    }
                    else
                    {
                        // Too many samples - just leave it alone!
                    }

                }
                ++pb; // move on 8 bits to next sound sample
            }
        }
    }
}
