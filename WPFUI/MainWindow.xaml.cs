﻿using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager
                .ConnectionStrings["WPFUI.Properties.Settings.PanjutorialsDBConnectionString"].ConnectionString;

            dataContext = new LinqToSqlDataClassesDataContext(connectionString);

            //InsertUniversities();

            //InsertStudents();

            //InsertLectures();

            //InsertStudentLectureAssociations();
            //GetUniversityOfToni();

            //GetToniLectures();

            //GetAllStudentsFromYale();

            //GetAllUniversitiesWithTransgenders();

            //GetAllLecturesFromBejingTech();
            //UpdateToni();
            //DeleteJames();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from University");

            University yale = new University();
            yale.Name = "Yale";
            dataContext.Universities.InsertOnSubmit(yale);

            University beijingTech = new University();
            beijingTech.Name = "Beijing Tech";
            dataContext.Universities.InsertOnSubmit(beijingTech);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }
        public void InsertStudents()
        {

            University yale = dataContext.Universities.First(
                un => un.Name.Equals("Yale"));
            University beijingTech = dataContext.Universities.First(
                un => un.Name.Equals("Beijing Tech"));

            List<Student> students = new List<Student>();

            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Toni", Gender = "male", University = yale });
            students.Add(new Student { Name = "Leyla", Gender = "female", University = beijingTech });
            students.Add(new Student { Name = "James", Gender = "trans-gender", University = beijingTech });


            dataContext.Students.InsertAllOnSubmit(students);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Lectures;
        }

        public void InsertStudentLectureAssociations()
        {
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));
            Student Leyla = dataContext.Students.First(st => st.Name.Equals("Leyla"));
            Student James = dataContext.Students.First(st => st.Name.Equals("James"));

            Lecture Math = dataContext.Lectures.First(lc => lc.Name.Equals("Math"));
            Lecture History = dataContext.Lectures.First(lc => lc.Name.Equals("History"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture
            {
                Student = Carla,
                Lecture = Math
            });

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture
            {
                Student = Toni,
                Lecture = Math
            });

            // Another way to insert
            StudentLecture slToni = new StudentLecture();
            slToni.Student = Toni;
            slToni.LectureId = History.Id;
            dataContext.StudentLectures.InsertOnSubmit(slToni);

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture
            {
                Student = Leyla,
                Lecture = History
            });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.StudentLectures;

        }

        public void GetUniversityOfToni()
        {
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            University ToniUniversity = Toni.University;
            List<University> universities = new List<University>();

            universities.Add(ToniUniversity);


            MainDataGrid.ItemsSource = universities;
        }


        public void GetToniLectures()
        {
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            var tonisLectures = from sl in Toni.StudentLectures
                                select sl.Lecture;


            MainDataGrid.ItemsSource = tonisLectures;
        }

        public void GetAllStudentsFromYale()
        {
            var studentsFromYale = from student in dataContext.Students
                                   where student.University.Name == "Yale"
                                   select student;

            MainDataGrid.ItemsSource = studentsFromYale;
        }

        public void GetAllUniversitiesWithTransgenders()
        {
            var transgenderUnivesities = from student in dataContext.Students
                                         join university in dataContext.Universities
                                         on student.University equals university
                                         where student.Gender == "trans-gender"
                                         select university;

            MainDataGrid.ItemsSource = transgenderUnivesities;
        }

        public void GetAllLecturesFromBejingTech()
        {
            var beijingLectures = from sl in dataContext.StudentLectures
                                  join student in dataContext.Students
                                  on sl.StudentId equals student.Id
                                  where student.University.Name == "Beijing Tech"
                                  select sl.Lecture;

            MainDataGrid.ItemsSource = beijingLectures;
        }

        public void UpdateToni()
        {
            Student Toni = dataContext.Students.FirstOrDefault(st
                => st.Name == "Toni");

            Toni.Name = "Antonio";

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteJames()
        {
            Student James = dataContext.Students.FirstOrDefault(st
                => st.Name == "James");

            dataContext.Students.DeleteOnSubmit(James);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }
    }
}
