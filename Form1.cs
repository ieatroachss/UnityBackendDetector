namespace GameDetector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string game = folderBrowserDialog1.SelectedPath;
                string backend = Detect(game);
                label4.Text = $"Detected: {backend}";
            }
        }

        private string Detect(string game)
        {
            string? data = null;
            string basedir = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var dir in Directory.GetDirectories(game))
            {
                if (dir.EndsWith("_Data"))
                {
                    data = dir;
                    break;
                }
            }

            if (data == null)
                return "No Data folder found.";

            string metadata = Path.Combine(data, "il2cpp_data", "Metadata", "global-metadata.dat");
            string gameassembly = Path.Combine(game, "GameAssembly.dll");
            string assembly_csharp = Path.Combine(data, "Managed", "Assembly-CSharp.dll");
            string il2cpplib1 = Path.Combine(data, "il2cpp", "libil2cpp.so");
            string il2cpplib2 = Path.Combine(data, "Plugins", "libil2cpp.so");
            string il2cpplib3 = Path.Combine(data, "Native", "libil2cpp.so"); 

            if (File.Exists(metadata) && File.Exists(gameassembly))
            {
                string di = Path.Combine(basedir, "IL2CPPScannedFiles");
                Directory.CreateDirectory(di);

                File.Copy(gameassembly, Path.Combine(di, "GameAssembly.dll"), true);
                File.Copy(metadata, Path.Combine(di, "global-metadata.dat"), true);

                if (File.Exists(il2cpplib1))
                    File.Copy(il2cpplib1, Path.Combine(di, "libil2cpp.so"), true);

                else if (File.Exists(il2cpplib2))
                    File.Copy(il2cpplib2, Path.Combine(di, "libil2cpp.so"), true);

                else if (File.Exists(il2cpplib3))
                    File.Copy(il2cpplib3, Path.Combine(di, "libil2cpp.so"), true);

                return "IL2CPP (files copied)";
            }
            else if (File.Exists(assembly_csharp))
            {
                string dm = Path.Combine(basedir, "MonoScannedFiles");
                Directory.CreateDirectory(dm);
                File.Copy(assembly_csharp, Path.Combine(dm, "Assembly-CSharp.dll"), true);

                return "Mono (file copied)";
            }
            else
            {
                return "Unknown.";
            }

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
