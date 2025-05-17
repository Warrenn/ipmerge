# ipmerge

**ipmerge** is a fast, native .NET tool that parses and merges overlapping IPv4 CIDR address spaces. It is optimized for scripting, automation, and Linux-native deployment using Ahead-of-Time (AOT) compilation.

---

## 🚀 Features

- Parses IP ranges in `IP/CIDR Label` format  
- Merges overlapping or adjacent ranges  
- Keeps the label of the first range in a merged group  
- Outputs simplified, non-overlapping ranges  
- Designed for shell usage (pipe input or file argument)  
- Built with .NET 8 Native AOT for zero-runtime deployment  

---

## 📦 Installation

### Download precompiled binary

Grab the latest release from the [Releases](https://github.com/your-org/ipmerge/releases) page:

```bash
wget https://github.com/your-org/ipmerge/releases/download/vX.Y.Z/ipmerge-linux-x64
chmod +x ipmerge-linux-x64
sudo mv ipmerge-linux-x64 /usr/local/bin/ipmerge
```

### Or build from source (requires Docker)

```bash
git clone https://github.com/your-org/ipmerge.git
cd ipmerge
./build.sh
```

The binary will be available in `./output/ipmerge`.

---

## 🧠 How it Works

1. Accepts input lines in the format:

   ```
   192.168.0.0/24 Office Network
   192.168.0.128/25 Guest Wifi
   ```

2. Converts each CIDR to a numeric IP range  
3. Merges overlapping or adjacent ranges  
4. Outputs a minimal set of non-overlapping CIDRs with preserved labels  

---

## 🛠 Usage

### From a file:

```bash
ipmerge ranges.txt
```

### From a pipe:

```bash
cat ranges.txt | ipmerge
```

### Example input:

```
100.64.0.0/10 LabelA
100.64.0.0/11 LabelB
```

### Example output:

```
100.64.0.0/10 LabelA
```

---

## 📄 Input Format

Each line must contain:

```
<IPv4>/<CIDR> <Label>
```

- IPv4: standard dotted format  
- CIDR: number from 0 to 32  
- Label: arbitrary string (may contain spaces)  

---

## 🔧 Command-line Options

```bash
ipmerge --help
```

```
Usage: ipmerge [file]

Merges overlapping IP ranges with labels.

Arguments:
  file      Optional path to input file. If not provided, reads from stdin.

Input Format:
  Each line: IP/CIDR Label
  Example:   192.168.0.0/24 Office LAN

Output Format:
  Merged IP/CIDR Label
```

---

## 🧪 Testing

```bash
dotnet test
```

Unit tests are under the `Tests/` project and use xUnit.

---

## 📦 Versioning

Versions are managed centrally via `Directory.Build.props`.

To release:

```bash
git tag v1.0.0
git push origin v1.0.0
```

GitHub Actions will build and publish a release binary automatically.

---

## 📜 License

MIT License. See [LICENSE](LICENSE).

---

## 🙏 Contributions

Contributions, issues, and suggestions welcome via pull requests or discussions.

---

## ✉️ Contact

Maintained by [@yourhandle](https://github.com/yourhandle).
