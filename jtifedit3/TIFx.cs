using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace jtifedit2.TIF {
    public class Reader {
        ReadLe rl;
        ReadBe rb;
        ISer br;
        Stream si;

        public Reader(Stream si) {
            this.si = si;
            rl = new ReadLe(si);
            rb = new ReadBe(si);

            ushort v = rl.ReadUInt16();
            if (v == 0x4949u) {
                br = rl;
            }
            else if (v == 0x4D4Du) {
                br = rb;
            }
            else throw new ReaderException(new InvalidDataException());

            if (br.ReadUInt16() != 42) throw new ReaderException(new InvalidDataException());

            diroff = br.ReadUInt32();
        }

        uint diroff;

        public Entry[] Read() {
            if (diroff == 0)
                return null;
            si.Position = diroff;
            int cx = br.ReadUInt16();
            List<Entry> al = new List<Entry>(cx);
            for (int x = 0; x < cx; x++) {
                al.Add(new Entry(br));
            }
            diroff = br.ReadUInt32();
            return al.ToArray();
        }

        internal interface ISer {
            byte ReadByte();
            ushort ReadUInt16();
            uint ReadUInt32();
        }

        class ReadBe : ISer {
            Stream si;
            byte[] b = new byte[4];

            public ReadBe(Stream si) {
                this.si = si;
            }

            #region ISer メンバ

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

            #endregion
        }

        class ReadLe : ISer {
            Stream si;
            byte[] b = new byte[4];

            public ReadLe(Stream si) {
                this.si = si;
            }

            #region ISer メンバ

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

            #endregion
        }
    }

    public class Entry {
        public ushort tag, fieldType;
        public uint count, offset;

        internal Entry(Reader.ISer br) {
            tag = br.ReadUInt16();
            fieldType = br.ReadUInt16();
            count = br.ReadUInt32();
            offset = br.ReadUInt32();
        }
    }

    public class ReaderException : ApplicationException {
        public ReaderException(Exception e)
            : base("読み取りに失敗しました", e) {

        }
    }
}
