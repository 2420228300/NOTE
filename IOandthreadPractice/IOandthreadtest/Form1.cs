using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Environment;
using static System.IO.Path;
using static System.IO.Directory;
using System.Collections;

namespace IOandthreadtest
{
    public delegate void openFloderHander(TreeView treeView, string fullpath);
    public partial class Form1 : Form
    {
        String filfloder = @"H:\Practice";
        public String saveFile =  @"H:\Practice\note";
        public string logpath= @"H:\Practice\LOG";
        ArrayList keyword = new ArrayList();
        public int cardname = 66;
        

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreatWorkFloder();
            addKeyWord("String");
            addKeyWord("static");


        }
        /// <summary>
        /// 创建工作目录
        /// </summary>
        public void CreatWorkFloder()
        {
            //日志保存目录
            CreateDirectory(filfloder+@"\LOG");
            //文件保存目录
            CreateDirectory(saveFile);
            //临时文件夹
            CreateDirectory(filfloder + @"\temp");
            if (Exists(filfloder))
            {
                addLog("工作目录已存在");
                //LOG.Items.Add("System:工作目录已存在");
            }
            else
            {
                CreateDirectory(filfloder);
                //文件保存目录
                CreateDirectory(saveFile);
                //日志保存目录
                CreateDirectory(filfloder + @"\log");
                if (Exists(filfloder))
                {
                    addLog("成功创建文件夹");
                    //LOG.Items.Add("System:成功创建文件夹");
                }
                else
                {
                    addLog("文件夹创建失败");
                    //LOG.Items.Add("System:文件夹创建失败");
                }
            }
            treeviewWork(treeView, @"H:\");
        }


        /// <summary>
        /// 采用递归实现向treeview中添加文件及目录
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="fullpath"></param>
        public void treeviewWork(TreeView treeView, string fullpath)
        {
            try
            {
                treeView.Nodes.Clear();

                DirectoryInfo dirs = new DirectoryInfo(fullpath);
                DirectoryInfo[] dir = dirs.GetDirectories();//获取当前目录下的子目录
                FileInfo[] file = dirs.GetFiles();   //放回当前目录的文件列表
                int dircount = dir.Count();  //获取文件夹对象数量
                int filecount = file.Count(); //获取文件数量
                //循环文件夹
                for (int i = 0; i < dircount; i++)
                {
                    treeView.Nodes.Add(dir[i].Name);
                    string pathnode = fullpath + "\\" + dir[i].Name;
                    GetMultiNode(treeView.Nodes[i], pathnode);
                    addLog("初始化文件夹");
                }
                //循环文件
                for (int j = 0; j < filecount; j++)
                {
                    treeView.Nodes.Add(file[j].Name);
                    addLog("初始化文件");
                    //LOG.Items.Add("System::初始化文件");
                }
            }
            catch (Exception q)
            {
                //MessageBox.Show("System::初始化错误" + q.Message);
            }
        }
        public bool GetMultiNode(TreeNode treeNode, string path)
        {
            if (Directory.Exists(path) == false)
            {
                return false;
            }
            DirectoryInfo dirs = new DirectoryInfo(path);
            DirectoryInfo[] dir = dirs.GetDirectories();
            FileInfo[] file = dirs.GetFiles();
            int dircount = dir.Count();
            int filecount = file.Count();
            int sumcount = dircount + filecount;
            if (sumcount == 0)
            {
                return false;
            }

            for (int i = 0; i < dircount; i++)
            {
                treeNode.Nodes.Add(dir[i].Name);
                String pathNodeB = path + "\\" + dir[i].Name;
                GetMultiNode(treeNode.Nodes[i], pathNodeB);
                addLog("初始化文件夹");
            }
            for (int i = 0; i < filecount; i++)
            {
                addLog("初始化文件");
                treeNode.Nodes.Add(file[i].Name);

            }
            return true;

        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            openfloder.Show(Location.X + 550, Location.Y + 300);
            addLog("点击了打开按钮");
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Location.X + 550, Location.Y + 300);
            addLog("点击了新建按钮");
        }


        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 创建文件_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(filename.Text.Trim()))
                {
                    TreeNode node = this.treeView.SelectedNode;
                    //获取当前树节点到根节点的路径
                    String path1 = node.FullPath;
                    String path2 = @"H:\";
                    String path = path2 + path1;
                    richTextBox.Text = path + @"\" + filename.Text;
                    if (File.Exists(path))
                    {
                        addLog("文件已存在1");
                        //LOG.Items.Add("System::文件已存在1");
                    }
                    else
                    {
                        treeView.SelectedNode.Nodes.Add(filename.Text);
                        File.Create(path + "\\" + filename.Text);
                        if (File.Exists(path + "\\" + filename.Text))
                        {
                            addLog("文件已存在2");
                            //LOG.Items.Add("System::文件已存在2");
                        }
                        else
                        {
                            addLog("文件创建失败");
                            //LOG.Items.Add("System::文件创建失败");
                        }
                    }

                }
                else
                {
                    MessageBox.Show("文件名字不可为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception eq)
            {
                MessageBox.Show(eq.Message);
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFloderHander openFloderHander = new openFloderHander(treeviewWork);
            openFloderHander(treeView, openpath.Text);
        }

        /// <summary>
        ///日志输出
        ///
        /// </summary>
        /// <param name="message"></param>
        public void addLog(String message)
        {
            bool scroll = false;
            if (this.LOG.TopIndex == this.LOG.Items.Count - (int)(this.LOG.Height / this.LOG.ItemHeight))
            scroll = true;
            this.LOG.Items.Add("System::"+message);
            if (scroll)
            this.LOG.TopIndex = this.LOG.Items.Count - (int)(this.LOG.Height / this.LOG.ItemHeight);
        }

 
        /// <summary>
        /// 文件保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "*.txt";   //设置文件默认扩展名
            saveFileDialog.Filter = "Txt Files|*.txt";  //设置筛选文件信息
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                addLog("文件保存成功");
            }
            else
            {
                addLog("文件保存失败");
            }
            
        }


        public void addKeyWord(String str)
        {
            keyword.Add(str);
        }

        /// <summary>
        /// 关键字变色
        /// </summary>
        public void txtChengeColor()
        {

        }

        private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 提示用户并保存日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DateTime date = DateTime.Now;
            string temp = @"\"+date.Year + "_" + date.Month + "_" + date.Day + "_" + date.Hour + "_" + date.Minute + "_" + date.Second + @".txt";
            StreamWriter fileStream = File.CreateText(logpath + temp);
            fileStream.WriteLine(date.Year + "年" + date.Month + "月" + date.Day + "日" + date.Hour + "小时" + date.Minute + "分钟" + date.Second + "秒");
            for (int i = 0; i < LOG.Items.Count; i++)
            {
                fileStream.WriteLine(LOG.Items[i]);
            }
           
        }


        /// <summary>
        /// 增加选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (cardname <91)
            {
                char title = (char)cardname;
                TabPage tabPage = new TabPage(title.ToString() + "卡");
                tabControl1.TabPages.Add(tabPage);
                addLog("增加选"+title+"项卡成功");
                cardname++;
            }
            else
            {
                MessageBox.Show("选项卡已经很多了", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                addLog("选项卡添加失败");
            }
        }
    }
      
}
