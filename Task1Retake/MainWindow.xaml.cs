using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Task1Retake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // create a new xml file eveery time the project runs
            string semseterFileName = $"sememster_info_{DateTime.Now:yyyyMMddHHmmss}.xml";
            string moduleFileName = $"module_info_{DateTime.Now:yyyyMMddHHmmss}.xml";

            ClearXmlFiles();

            //Load and display both semester and module data
            LoadAndDisplayData(semseterFileName, moduleFileName);
        }

        

        private void AppendSemesterToXml(Semester semester, string filePath)
        {
            try
            {
                // Deserialize existing semester data from the XML file
                List<Semester> existingSem = DeserializeFromXml(filePath) ?? new List<Semester>();

                // Add the semester to the list of semesters
                existingSem.Add(semester);

                // Serialize and save the updated semester data back to the XML file
                using (var writer = new StreamWriter(filePath))
                {
                    var serializer = new XmlSerializer(typeof(List<Semester>));
                    serializer.Serialize(writer, existingSem);
                }
                MessageBox.Show("Semester information added to the XML file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding semester data: {ex.Message}");
            }
        }

        private List<Semester> DeserializeFromXml(string filePath)
        {
            // for the semester

            try
            {
                if (File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        var serializer = new XmlSerializer(typeof(List<Semester>));
                        return (List<Semester>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading existing semester data: {ex.Message}");
            }
            return null;
        }

        private void addModule_Click(object sender, RoutedEventArgs e)
        {
            string semesterName = ((ComboBoxItem)semBox.SelectedItem).Content.ToString();
            if (!int.TryParse(weekBox1.Text, out int numberOfWeeks))
            {
                MessageBox.Show("Invalid format - type in numerics");
                return;
            }
            DateTime? startDate = dates.SelectedDate;
            if (!startDate.HasValue)
            {
                MessageBox.Show("Please select a valid date");
                return;
            }

            // use the info to create a semester
            var semInfo = new Semester
            {
                SemesterName = semesterName,
                NumofWeeks = numberOfWeeks,
                StartDate = startDate.Value
            };

            string moduleCode = codeBox.Text;
            string moduleName = nameBox.Text;
            int numOfCredits = int.Parse(creditBox.Text);
            int classHours = int.Parse(classHoursBox.Text);


            int ss = CalcSelffStudy(semInfo,numOfCredits, classHours);

            var moduleInfo = new Module
            {
                ModuleCode = moduleCode,
                ModuleName = moduleName,
                NumOfCredits = numOfCredits,
                ClassHours = classHours,
                SelfStudy = ss
            };

            AppendModuleToXml(moduleInfo, "module_info.xml");
            LoadAndDisplayData("semester_info.xml", "module_info.xml");
        }
        private int CalcSelffStudy(Semester semester, int noCredits, int ClassHours)
        {
            int ss = 0;

            ss = ((noCredits * 10) / semester.NumofWeeks) - ClassHours;

            return ss;
        }

        private void AppendModuleToXml(Module moduleInfo, string filePath)
        {
            
            try
            {
                // Deserialize existing module data from the XML file
                List<Module> existingModules = DeserializedModulesFromXml(filePath) ?? new List<Module>();

                // Add the module to the list of modules
                existingModules.Add(moduleInfo);

                // Serialize and save the updated module data back to the XML file
                using (var writer = new StreamWriter(filePath))
                {
                    var serializer = new XmlSerializer(typeof(List<Module>));
                    serializer.Serialize(writer, existingModules);
                }
                MessageBox.Show("Module information added to the XML file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding module data: {ex.Message}");
            }
        }

        private List<Module> DeserializedModulesFromXml(string filePath)
        {
            try
            {
                if(File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        var serializer = new XmlSerializer(typeof(List<Module>));
                        return (List<Module>)serializer.Deserialize(reader);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"Error reading existing module data: {ex.Message}");
            }
            return null;
        }

        private void LoadAndDisplayData(string semesterFileName, string moduleFileName)
        {
            List<Semester> semesters = DeserializeFromXml(semesterFileName) ?? new List<Semester>();
            List<Module> modules = DeserializedModulesFromXml(moduleFileName) ?? new List<Module>();

            // Clear the DataGrid
            gridModlues.ItemsSource = null;

            
            gridModlues.ItemsSource = modules;
        }

        private void ClearXmlFiles()
        {
            // the file paths for the two XML files
            string semesterFilePath = "semester_info.xml";
            string moduleFilePath = "module_info.xml";



            try
            {
               
                if (File.Exists(semesterFilePath))
                {
                    File.Delete(semesterFilePath);
                }



                if (File.Exists(moduleFilePath))
                {
                    File.Delete(moduleFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing XML files: {ex.Message}");
            }
        }

        private void gridModlues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string modulecode = mCode.Text;
            string hours = ssHours.Text;

            List<Module> datagrid = new List<Module>();
           
            //foreach (Module module in gridModlues.Items)
            //{
            //    datagrid.Add(module);
            //}
            foreach (object item in gridModlues.Items)
            {
                if (item is Module gridModule)
                {
                    if (gridModule.ModuleCode == modulecode)
                    {
                        gridModule.SelfStudy = gridModule.SelfStudy - Convert.ToInt32(hours);
                        break; 
                    }
                }
            }

           

            gridModlues.Items.Refresh();

        }
        //retake --> seialization
        // xml documents --> semester
    }//class ends

    [Serializable]
    public class Data
    {
        //allow you to create semester & module
        public List<Semester> Semesters { get; set; }
        public List<Module> Modules { get; set; }
    }
}
