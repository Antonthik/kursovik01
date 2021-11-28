using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lesson9_task0
{
    class Program
    {

        static void Main(string[] args)
        {
            int page = 0;
            string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            string infofile = "";
            Param param0 = new Param(workDir, 0);
            //Param param0 = new Param(workDir, 0);
            //serial_json(param0, "param.json");//запись параметров программы
            readParam(ref param0); //чтение файла с параметрами программы
            serial_json(param0, "param.json");//запись параметров программы
            workDir =param0.CurrentPuth;
            page = param0.Page;


           while (true)
           {
               
                Console.Clear();//чистка экрана
                Display(param0.Page,ref infofile, workDir);//рисуем интерфейс программы
                Console.Write("введите команду:");
                string input = Console.ReadLine();

                //Выход из программы по запросу пользователя
                if (input == "exit")
                {
                    return;
                }
                else if(input != "")
                {
                    commadSelect(input,ref page,ref  infofile,ref workDir);//селектор команд
                    param0.Page = page;
                    param0.CurrentPuth = workDir;
                    serial_json(param0,"param.json");//запись параметров программы


                }
            }

        }

        static void commadSelect(string input,ref int page, ref string info,ref string workDir)
        {

            try
            {
                if (input.Substring(0, 2) == "cf")//копируем файл
                {
                    if (copyFile(input, ref workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                else if (input.Substring(0, 2) == "cd")//копируем директорию
                {
                    if (copyFolder(input, workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                else if (input.Substring(0, 2) == "rd")//удаление директории
                {
                    if (delFolder(input, workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                else if (input.Substring(0, 2) == "rf")//удаление файла
                {
                    if (delFile(input, workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                else if (input.Substring(0, 4) == "file")//параметры файла
                {
                    if (infoFile(input, ref info, workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                else if (input.Substring(0, 2) == "pg")//перелистование
                {
                    if (pageList(input, ref page, workDir) == true) { Console.WriteLine("ошибка команды"); }

                }
                else if (input.Substring(0, 2) == "nd")//новыя директория
                {
                    if (newDir(input, ref workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                else if (input.Substring(0, 4) == "fold")//параметры файла
                {
                    if (infoFold(input, ref info, workDir) == true) { Console.WriteLine("ошибка команды"); }
                }
                

                
            }
            catch (Exception)
            {

                Console.WriteLine("ошибка команды");
            }


        }
        //копирование файла
        static bool newDir(string str,ref string workDir)
        {

            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 2)
            {

                try
                {
                    if (Directory.Exists(arr[1]) == true)
                    {
                        workDir = arr[1];
                    }
                   
                }
                catch (Exception)
                {

                    flagErr = true;
                }

            }
            else
            {
                flagErr = true;
            }


                return flagErr;
            

        }

        //копирование файла
        static bool copyFile(string str,ref string workDir)
        {
            
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 3){
                try
                {
                     File.Copy(workDir+arr[1], workDir+arr[2]);
                }
                catch (Exception)
                {

                    flagErr = true;
                }
               
            }
            else
            {
                flagErr = true;
            }


            return flagErr;

        }

        //копирование директории
        static bool copyFolder(string str, string workDir)
        {
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 3)
            {
                if (Directory.Exists(workDir + arr[2]) == false)
                {
                    //создаем директорию
                    Directory.CreateDirectory(workDir + arr[2]);
                }

                //копируем директории
                    foreach (string dirPath in Directory.GetDirectories(workDir + arr[1], "*",SearchOption.AllDirectories))
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath.Replace(workDir + arr[1], workDir + arr[2]));
                    }
                    catch (Exception)
                    {
                        flagErr = true;
                    }
                }
                //Копировать все файлы и перезаписать файлы с идентичным именем
                foreach (string newPath in Directory.GetFiles(workDir + arr[1], "*.*",SearchOption.AllDirectories))
                {
                    try
                    {
                        File.Copy(newPath, newPath.Replace(workDir + arr[1], workDir + arr[2]), true);
                    }
                    catch (Exception)
                    {
                        flagErr = true;
                    }
                }

            }
            else
            {
                flagErr = true;
            }


            return flagErr;

        }

        //Удаление файла
        static bool delFile(string str, string workDir)
        {
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 2)
            {
                try
                {
                    File.Delete(workDir + arr[1]);
                }
                catch (Exception)
                {

                    flagErr = true;
                }

            }
            else
            {
                flagErr = true;
            }


            return flagErr;

        }

        //Удаление папки
        static bool delFolder(string str, string workDir)
        {
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 2)
            {
                try
                {  
                        delFolder_recurs( workDir + arr[1]);
                        if (Directory.Exists(workDir + arr[1]) == true) { Directory.Delete(workDir + arr[1]); }                    
                }
                catch (Exception)
                {

                    flagErr = true;
                }

            }
            else
            {
                flagErr = true;
            }


            return flagErr;

        }

        static bool infoFile(string str,ref string info, string workDir)
        {
           //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 2)
            {
                try
                {
                    string path = Path.Combine(workDir, arr[1]);

                    //FileInfo file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    FileInfo file = new FileInfo(workDir + arr[1]);

                    //Console.WriteLine( $"Information about file: {file.Name}, {file.Length} bytes, last modified on {file.LastWriteTime} - Full path: {file.FullName}");
                    //info = $"Information about file: {file.Name}, {file.Length} bytes, last modified on {file.LastWriteTime} - Full path: {file.FullName}";
                    info = $"Info file: {file.Name}, {file.Length} bytes, last modified on {file.LastWriteTime}";
                }
                catch (Exception)
                {

                    flagErr = true;
                }
                    
 
            }


            return flagErr;

        }
        static bool infoFold(string str, ref string info, string workDir)
        {
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 2)
            {
                try
                {
                    string path = Path.Combine(workDir, arr[1]);

                    //FileInfo file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    DirectoryInfo fold = new DirectoryInfo(workDir + arr[1]);

                    //Console.WriteLine( $"Information about file: {file.Name}, {file.Length} bytes, last modified on {file.LastWriteTime} - Full path: {file.FullName}");
                    //info = $"Information about file: {file.Name}, {file.Length} bytes, last modified on {file.LastWriteTime} - Full path: {file.FullName}";
                    info = $"Info fold: {fold.Name}, CreationTime on {fold.CreationTime}, last modified on {fold.LastWriteTime}";
                }
                catch (Exception)
                {

                    flagErr = true;
                }


            }


            return flagErr;

        }
        static bool pageList(string str,ref int page, string workDir)
        {
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            bool flagErr = false;
            string[] arr = str.Split(" ");
            if (arr.Length == 2)
            {
                try
                {
                    page = Convert.ToInt32(arr[1]);
                }
                catch (Exception)
                {

                    flagErr = true;
                }
               
            }
            else
            {
                flagErr = true;
            }


            return flagErr;

        }

        //Удаление папки со всеми вложениями рекурсией
        static bool delFolder_recurs(string director) 
        {
            
            bool flagErr = false;
           
            try
            {
                DirectoryInfo dir = new DirectoryInfo(director);
                DirectoryInfo[] arrDir = dir.GetDirectories();
                FileInfo[] arrFiles = dir.GetFiles();

                foreach (FileInfo file in arrFiles)
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in arrDir)
                {

                    delFolder_recurs(folder.FullName);


                    if (folder.GetDirectories().Length == 0 && folder.GetFiles().Length == 0)
                    {
                        folder.Delete();
                    }
                }
            }
            catch (Exception)
            {

                flagErr = true;
            }

            return flagErr;
        }

        static void Display(int page,ref string fileinfo,string workDir)
        {
            //string workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

            if (fileinfo.Length > 98) { 
                fileinfo.Substring(0, 98);
            }

            string[] arr = new string[200];
            int count=0;
            int j = 0;
            arr=arrDir(workDir, arr,out count);
            string[] arrDirect = new string[count];
            Array.Copy(arr, arrDirect, count);
            if (count > 0)
            {
                //int page = 0;
                int countStrOfpage = 30;//количество строк в листе

                j = countStrOfpage * page;// перелист

                //Console.WriteLine(new string((char)205'-',90));
                String line1 = ($"{'\u2554'}{new string('\u2550', 98)}{'\u2557'}");
                String line2 = ($"{'\u2551'}{new string(' ', 98)}{'\u2551'}");
                String line3 = ($"{'\u2560'}{new string('\u2550', 98)}{'\u2563'}");
                String line4 = ($"{'\u255a'}{new string('\u2550', 98)}{'\u255d'}");

                string lineInfo1 = "команда cf - копирование файла";
                string lineInfo2 = "команда cd - копирование директории";
                string lineInfo3 = "команда rd - удаление директории";
                string lineInfo4 = "команда rf - удаление файла";
                string lineInfo5 = "команда file - информация файла";
                string lineInfo6 = "команда pg - перелистование дерева";
                string lineInfo7 = "команда nd - выбор новой директории";
                string lineInfo8 = "команда fold - информация папки";

                //основная область
                Console.WriteLine((line1));

                for (int i = 0; i < 40; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine(($"{'\u2551'}{arrDirect[0]}{new string(' ', 98 - (arrDirect[0].Length))}{'\u2551'}"));
                        j++;
                    }
                    if (i > 0 && i <= 30)
                    {


                        if (j <= arrDirect.Length - 1)
                        {
                            Console.WriteLine(($"{'\u2551'}{arrDirect[j]}{new string(' ', 98 - (arrDirect[j].Length))}{'\u2551'}"));//строка с данными
                            j++;
                        }
                        else
                        {
                            Console.WriteLine(($"{'\u2551'}{new string(' ', 98)}{'\u2551'}"));//пустая строка
                        }

                    }

                    if (i == 31) { Console.WriteLine((line3)); }

                    if (i >= 31 && i <= 31)
                    {
                        //Console.WriteLine((line2));
                        int hhh = fileinfo.Length;
                        Console.WriteLine($"{'\u2551'}{fileinfo}{new string(' ', 98 - (fileinfo.Length))}{'\u2551'}");//строка с данными
                    }

                    if (i >= 32 && i <= 32) { Console.WriteLine(($"{'\u2551'}{lineInfo1}{new string(' ', 98 - (lineInfo1.Length))}{'\u2551'}")); }
                    if (i >= 33 && i <= 33) { Console.WriteLine(($"{'\u2551'}{lineInfo2}{new string(' ', 98 - (lineInfo2.Length))}{'\u2551'}")); }
                    if (i >= 34 && i <= 34) { Console.WriteLine(($"{'\u2551'}{lineInfo3}{new string(' ', 98 - (lineInfo3.Length))}{'\u2551'}")); }
                    if (i >= 35 && i <= 35) { Console.WriteLine(($"{'\u2551'}{lineInfo4}{new string(' ', 98 - (lineInfo4.Length))}{'\u2551'}")); }
                    if (i >= 36 && i <= 36) { Console.WriteLine(($"{'\u2551'}{lineInfo5}{new string(' ', 98 - (lineInfo5.Length))}{'\u2551'}")); }
                    if (i >= 37 && i <= 37) { Console.WriteLine(($"{'\u2551'}{lineInfo6}{new string(' ', 98 - (lineInfo6.Length))}{'\u2551'}")); }
                    if (i >= 38 && i <= 38) { Console.WriteLine(($"{'\u2551'}{lineInfo7}{new string(' ', 98 - (lineInfo7.Length))}{'\u2551'}")); }
                    if (i >= 39 && i <= 39) { Console.WriteLine(($"{'\u2551'}{lineInfo8}{new string(' ', 98 - (lineInfo8.Length))}{'\u2551'}")); }


                }
                Console.WriteLine((line4));
            }
   

           
        }
        static string[] arrDir(string director, string[] arrDirect,out int i)
        {
             i = 0;
            
            try
            {
                DirectoryInfo dir = new DirectoryInfo(director);
                DirectoryInfo[] arrDir = dir.GetDirectories();
                arrDirect[i] = $"{director}"; //пишем в массив и убираем корневые папки
                i = i + 1;

                foreach (DirectoryInfo folder in arrDir)
                {
                    if (i > 200) break;

                    arrDirect[i] = $"  |-->{folder.Name}"; //пишем в массив и убираем корневые папки
                    i = i + 1;
                    // Второй уровень директорий
                    DirectoryInfo dir2 = new DirectoryInfo(folder.FullName);
                    try
                    {
                        DirectoryInfo[] arrDir2 = dir2.GetDirectories();
                        foreach (DirectoryInfo folder2 in arrDir2)
                        {
                            if (i > 200) break;
                            arrDirect[i] = $"  |      |-->{folder2.Name}";//пишем в массив и убираем корневые папки
                            i = i + 1;
                        }

                    }
                    catch (Exception)
                    {


                    }


                }
            }
            catch (Exception)
            {

               // workDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            }
          

            

            return arrDirect;
        }

        //сериализация json
        static void serial_json(Param task, string fileName)
        {
            
            string pathDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            string pathFile = Path.Combine(pathDir, fileName);
            if (File.Exists(pathFile) == true) { File.Delete(pathFile); }

            string json = JsonSerializer.Serialize(task);
            File.WriteAllText(pathFile, $"{json}");
        }

        //Читаем задачи,десериализация json
        static Param readTasks(string fileName)
        {
            string pathDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            string pathFile = Path.Combine(pathDir, fileName);

           
                string json = File.ReadAllText(pathFile);                
                    Param task = JsonSerializer.Deserialize<Param>(json);                    
                  
                    return task;
         
        }

        static void readParam(ref Param param0)
        {
            string pathDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            string pathFile = Path.Combine(pathDir, "param.json");
            if (File.Exists(pathFile) == true)
            {
                Param param1 = readTasks("param.json");
                param0.CurrentPuth = param1.CurrentPuth;
                param0.Page = param1.Page;
            }
        }
    }
}
