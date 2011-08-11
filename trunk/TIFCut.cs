using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace jtifedit3.TC {
    public class TIFCut {
        Stream si;
        ISer br;

        public void Cut(Stream fs) {
            this.si = fs;
            RWLe rl = new RWLe(si);
            RWBe rb = new RWBe(si);

            ushort v = rl.ReadUInt16();
            if (v == 0x4949u) {
                br = rl;
            }
            else if (v == 0x4D4Du) {
                br = rb;
            }
            else throw new ReaderException(new InvalidDataException());

            if (br.ReadUInt16() != 42) throw new ReaderException(new InvalidDataException());

            uint diroff = br.ReadUInt32();
            while (true) {
                diroff = EditDir(diroff);
                if (diroff == 0) break;
            }
        }

        private uint EditDir(uint diroff) {
            si.Position = diroff;
            int cx = br.ReadUInt16();
            for (int x = 0; x < cx; x++) {
                Int64 entoff = si.Position;
                ushort tag = br.ReadUInt16();
                ushort fieldType = br.ReadUInt16();
                uint count = br.ReadUInt32();
                uint offset = br.ReadUInt32();

                switch (tag) {
                    case 0x8769:
                    case 0x8825:
                    case 0x8773:
                    case 0xA005:
                        tag = 0;
                        si.Position = entoff;
                        br.Write(tag);
                        br.Write(fieldType);
                        br.Write(count);
                        br.Write(offset);
                        break;
                }
            }
            diroff = br.ReadUInt32();
            return diroff;
        }

        internal interface ISer {
            byte ReadByte();
            ushort ReadUInt16();
            uint ReadUInt32();

            void Write(byte b);
            void Write(ushort w);
            void Write(uint ww);
        }

        class RWBe : ISer {
            Stream si;
            byte[] b = new byte[4];

            public RWBe(Stream si) {
                this.si = si;
            }

            #region ISer ÉÅÉìÉo

            public byte ReadByte() {
                if (1 != si.Read(b, 0, 1)) throw new ReaderException(new EndOfStreamException());
                return b[0];
            }

            public ushort ReadUInt16() {
                if (2 != si.Read(b, 0, 2)) throw new ReaderException(new EndOfStreamException());
                return (ushort)(b[1] | (b[0] << 8));
            }

            public uint ReadUInt32() {
                if (4 != si.Read(b, 0, 4)) throw new ReaderException(new EndOfStreamException());
                return (uint)(b[3] | (b[2] << 8) | (b[1] << 16) | (b[0] << 24));
            }

            public void Write(byte b) {
                si.WriteByte(b);
            }

            public void Write(ushort w) {
                si.WriteByte((byte)(w >> 8));
                si.WriteByte((byte)(w));
            }

            public void Write(uint ww) {
                si.WriteByte((byte)(ww >> 24));
                si.WriteByte((byte)(ww >> 16));
                si.WriteByte((byte)(ww >> 8));
                si.WriteByte((byte)(ww));
            }

            #endregion
        }

        class RWLe : ISer {
            Stream si;
            byte[] b = new byte[4];

            public RWLe(Stream si) {
                this.si = si;
            }

            #region ISer ÉÅÉìÉo

            public byte ReadByte() {
                if (1 != si.Read(b, 0, 1)) throw new ReaderException(new EndOfStreamException());
                return b[0];
            }

            public ushort ReadUInt16() {
                if (2 != si.Read(b, 0, 2)) throw new ReaderException(new EndOfStreamException());
                return (ushort)(b[0] | (b[1] << 8));
            }

            public uint ReadUInt32() {
                if (4 != si.Read(b, 0, 4)) throw new ReaderException(new EndOfStreamException());
                return (uint)(b[0] | (b[1] << 8) | (b[2] << 16) | (b[3] << 24));
            }

            public void Write(byte b) {
                si.WriteByte(b);
            }

            public void Write(ushort w) {
                si.WriteByte((byte)(w));
                si.WriteByte((byte)(w >> 8));
            }

            public void Write(uint ww) {
                si.WriteByte((byte)(ww));
                si.WriteByte((byte)(ww >> 8));
                si.WriteByte((byte)(ww >> 16));
                si.WriteByte((byte)(ww >> 24));
            }

            #endregion
        }
    }

    public class ReaderException : ApplicationException {
        public ReaderException(Exception e)
            : base("ì«Ç›éÊÇËÇ…é∏îsÇµÇ‹ÇµÇΩ", e) {

        }
    }
}
