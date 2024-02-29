using FGSIWinTool.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FGSIWinTool
{
    public partial class ScriptFile
    {
        private byte[] _codeBuffer;
        private ScriptHeader _header;
        private List<int> _labels;
        private readonly FGSIWinAssembly _assembly;

        public ScriptFile()
        {
            _codeBuffer = [];
            _header = new ScriptHeader();
            _labels = [];
            _assembly = new FGSIWinAssembly();
        }

        public void Load(string path)
        {
            var stream = File.OpenRead(path);
            var reader = new BinaryReader(stream);

            // Read header

            _header.Magic = reader.ReadInt32();

            if (_header.Magic != 0x45524353)
            {
                throw new Exception("Not a valid script file.");
            }

            _header.Unknow = reader.ReadInt32();
            _header.LabelCount = reader.ReadInt32();
            _header.LabelPos = reader.ReadInt32();
            _header.CodePos = reader.ReadInt32();

            // Read labels

            if (_header.LabelCount > 0)
            {
                _labels = new List<int>(_header.LabelCount);

                stream.Position = _header.LabelPos;

                for (var i = 0; i < _header.LabelCount; i++)
                {
                    _labels.Add(reader.ReadInt32());
                }
            }

            // Read code data

            var codeLength = Convert.ToInt32(stream.Length - stream.Position);
            _codeBuffer = reader.ReadBytes(codeLength);

            Disassemble();

            // Finish

            stream.Dispose();
        }

        public void Save(string path)
        {
            var stream = File.Create(path);
            var writer = new BinaryWriter(stream);

            writer.Write(_header.Magic);
            writer.Write(_header.Unknow);
            writer.Write(_header.LabelCount);
            writer.Write(0); // label pos
            writer.Write(0); // code pos

            // Write labels

            _header.LabelPos = Convert.ToInt32(stream.Position);

            for (var i = 0; i < _labels.Count; i++)
            {
                writer.Write(_labels[i]);
            }

            // Write code data

            _header.CodePos = Convert.ToInt32(stream.Position);

            writer.Write(_codeBuffer);

            // Update header

            stream.Position = 0xC;
            writer.Write(_header.LabelPos);
            stream.Position = 0x10;
            writer.Write(_header.CodePos);

            // Finish

            writer.Flush();
            stream.Dispose();
        }

        private void Disassemble()
        {
            var stream = new MemoryStream(_codeBuffer);
            var reader = new BinaryReader(stream);

            var stack = new List<int>(16);

            while (stream.Position < stream.Length)
            {
                var addr = stream.Position;
                var code = (FGSIWinInstruction)reader.ReadByte();

                switch (code)
                {
                    case FGSIWinInstruction.Unused:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Throw:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op0A:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op0B:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op0C:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op0D:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op0E:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op0F:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op10:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op11:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Msg:
                    {
                        reader.ReadNullTerminatedString();
                        break;
                    }
                    case FGSIWinInstruction.CJmp:
                    {
                        reader.ReadInt32BigEndian(); // label
                        break;
                    }
                    case FGSIWinInstruction.Jmp:
                    {
                        reader.ReadInt32BigEndian(); // label
                        break;
                    }
                    case FGSIWinInstruction.CallFunc:
                    {
                        var funcId = stack[0];

                        switch (funcId)
                        {
                            case 0x00:
                            case 0x01:
                            case 0x02:
                            case 0x03:
                            case 0x04:
                            case 0x0A:
                                break;
                            case 0x0B: // CFuncLibrary
                            case 0x0C: // CFuncPackage
                            case 0x0D: // CFuncUserAnime
                            case 0x0E: // CFuncTask
                            {
                                switch (stack[1])
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        reader.ReadInt32BigEndian();
                                        break;
                                    case 2:
                                    case 3:
                                        break;
                                    default:
                                        throw new Exception("Unknow type.");
                                }
                                break;
                            }
                            case 0x0F:
                            case 0x10:
                            case 0x11:
                            case 0x12:
                            case 0x13:
                            case 0x14:
                            case 0x16:
                            case 0x17:
                            case 0x18:
                            case 0x19:
                            case 0x1A:
                            case 0x1B:
                            case 0x1C:
                            case 0x1D:
                            case 0x1E:
                            case 0x1F:
                            case 0x20:
                            case 0x21:
                            case 0x22:
                            case 0x23:
                            case 0x24:
                            case 0x25:
                            case 0x26:
                            case 0x27:
                            case 0x28:
                            case 0x29:
                            case 0x2B:
                            case 0x32:
                            case 0x33:
                            case 0x34:
                            case 0x64:
                            case 0xC8:
                                break;
                            default:
                                throw new Exception("Unknow function id.");
                        }

                        break;
                    }
                    case FGSIWinInstruction.Op18:
                    {
                        reader.ReadInt32BigEndian(); // index
                        reader.ReadInt32BigEndian(); // zero
                        break;
                    }
                    case FGSIWinInstruction.Call:
                    {
                        reader.ReadInt32BigEndian(); // label
                        break;
                    }
                    case FGSIWinInstruction.Op1A:
                    {
                        reader.ReadByte();
                        break;
                    }
                    case FGSIWinInstruction.PushDword:
                    {
                        var v = reader.ReadInt32BigEndian();

                        stack.Insert(0, v);

                        if (stack.Count > 3)
                            stack.RemoveAt(stack.Count - 1);

                        break;
                    }
                    case FGSIWinInstruction.PushString:
                    {
                        reader.ReadNullTerminatedString();
                        break;
                    }
                    case FGSIWinInstruction.Add:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Sub:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Mul:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Div:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Mod:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op39:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op3A:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op3B:
                    {
                        break;
                    }
                    case FGSIWinInstruction.And:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Or:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Lt:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Gt:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Le:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Ge:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Eq:
                    {
                        break;
                    }
                    case FGSIWinInstruction.Op43:
                    {
                        break;
                    }
                    default:
                    {
                        throw new InvalidDataException("Unknow opcode.");
                    }
                }

                var length = Convert.ToInt32(stream.Position - addr);
                _assembly.Add(code, addr, length);
            }
        }

        public void ExportText(string path)
        {
            var writer = File.CreateText(path);

            var stream = new MemoryStream(_codeBuffer);
            var reader = new BinaryReader(stream);

            foreach (var rec in _assembly)
            {
                if (rec.Instruction == FGSIWinInstruction.Msg ||
                    rec.Instruction == FGSIWinInstruction.PushString)
                {
                    stream.Position = rec.Addr + 1;

                    var s = reader.ReadNullTerminatedString();

                    if (!string.IsNullOrWhiteSpace(s) && s[0] > 0x80)
                    {
                        writer.WriteLine("◇{0:X8}◇{1}", rec.Addr, s);
                        writer.WriteLine("◆{0:X8}◆{1}", rec.Addr, s);
                        writer.WriteLine();

                    }
                }
            }

            writer.Flush();
            writer.Dispose();
        }

        [GeneratedRegex(@"◆(\w+)◆(.+$)")]
        private static partial Regex TextLineRegex();

        private static Dictionary<long, string> LoadTranslation(string path)
        {
            using var reader = File.OpenText(path);

            var dict = new Dictionary<long, string>();
            var num = 0;

            while (!reader.EndOfStream)
            {
                var n = num;
                var line = reader.ReadLine();
                num++;

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line[0] != '◆')
                {
                    continue;
                }

                var match = TextLineRegex().Match(line);

                if (match.Groups.Count != 3)
                {
                    throw new Exception($"Bad format at line: {n}");
                }

                var addr = long.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
                var text = match.Groups[2].Value.Unescape();

                dict.Add(addr, text);
            }

            reader.Close();

            return dict;
        }

        public void ImportText(string path)
        {
            Console.WriteLine("Loading translation...");

            var translation = LoadTranslation(path);

            Console.WriteLine("Preparing to rebuild...");

            var instMap = _assembly.Instructions.ToDictionary(x => x.Addr);

            var codeStream = new MemoryStream(_codeBuffer);
            var codeReader = new BinaryReader(codeStream);

            var outStream = new MemoryStream(_codeBuffer.Length * 2);
            var outWriter = new BinaryWriter(outStream);

            Console.WriteLine("Building code...");

            foreach (var rec in _assembly.Instructions)
            {
                rec.NewAddr = Convert.ToInt32(outStream.Position);

                if (rec.Instruction == FGSIWinInstruction.Msg ||
                    rec.Instruction == FGSIWinInstruction.PushString)
                {
                    if (translation.TryGetValue(rec.Addr, out string? text))
                    {
                        var bytes = Encoding.UTF8.GetBytes(text);

                        outWriter.Write((byte)rec.Instruction);
                        outWriter.Write(bytes);
                        outWriter.Write((byte)0);
                    }
                    else
                    {
                        outWriter.Write(_codeBuffer, rec.Addr, rec.Length);
                    }
                }
                else
                {
                    outWriter.Write(_codeBuffer, rec.Addr, rec.Length);
                }
            }

            Console.WriteLine("Fixing code...");

            foreach (var rec in _assembly.Instructions)
            {
                if (rec.Instruction == FGSIWinInstruction.CJmp ||
                    rec.Instruction == FGSIWinInstruction.Jmp ||
                    rec.Instruction == FGSIWinInstruction.Call)
                {
                    codeStream.Position = rec.Addr + 1;
                    outStream.Position = rec.NewAddr + 1;

                    var dest = codeReader.ReadInt32BigEndian();
                    var newDest = instMap[dest].NewAddr;

                    // Console.WriteLine("Fix offset at {0:X8} : {1:X8} -> {2:X8}", outCodeStream.Position, dest, newDest);

                    outWriter.WriteInt32BigEndian(newDest);
                }
            }

            Console.WriteLine("Updating labels...");

            for (var i = 0; i < _labels.Count; i++)
            {
                _labels[i] = instMap[_labels[i]].NewAddr;
            }

            Console.WriteLine("Flush code...");

            _codeBuffer = outStream.ToArray();

            Console.WriteLine("Rebuild finished.");
        }
    }
}
