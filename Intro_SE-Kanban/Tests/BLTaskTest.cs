using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DalObjects;

namespace Tests
{

    public class Tests
    {
        Mock<BLTask> taskMock;
        BLTask TaskTest;
        Service service;
        DalTask dalT;
        Mock<DalTask> dalMock;
        string assignee = "bla@gmail.com";

        [SetUp]
        public void Setup()
        {

            service = new Service();
            service.Register("bla@gmail.com", "aA1234", "bla"); //for database creation
            dalT = new DalTask();
            taskMock = new Mock<BLTask>();
            dalMock = new Mock<DalTask>();
            TaskTest = new BLTask();
            //for last test usage only
            DateTime time = new DateTime(2021, 1, 1);
            TaskTest = new BLTask(1, 1, 1, 1, "title", "descripsion", DateTime.Now, time, assignee);
            dalT = new DalTask(1, 1, 1, 1, "title", "descripsion", DateTime.Now, time, assignee);
            dalT.Insert();
        }

        [TearDown]
        public void TearDown()
        {
            service.DeleteData();
        }

        //the prpose of this test is to see thet the illegal titles are failing and an argumentException is trowne

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("this title is way to long and is probebly way moor then 50 caracters at lest i hope")]
        public void TitleIsLegalTestFalse(string title)
        {
            Assert.Throws<ArgumentException>(() => TaskTest.TitleIsLegal(title));
        }

        //the prpose of this test is to see thet the legal titles are passing and are returning true
        [Test]
        [TestCase("normal titel")]
        public void TitleIsLegalTestTrue(string title)
        {
            bool res;
            string exception = "";
            //act
            try
            {
                res = TaskTest.TitleIsLegal(title);

            }
            catch (Exception e)
            {
                res = false;
                exception = e.Message;
            }
            //expected
            bool expected = true;
            //assert
            Assert.AreEqual(expected, res, $"Description Is legal test failed : {exception}");
        }


        //the prpose of this test is to see thet the legal Description are passing and are returning true
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("cool Task")]

        public void DescriptionIsLegalTest(String description)
        {
            bool res;
            string exception = "";
            //act
            try
            {
                 res = TaskTest.DescriptionIsLegal(description);
               
            }
            catch(Exception e)
            {
                res = false;
                exception = e.Message;
            }
            //expected
            bool expected = true;
            //assert
            Assert.AreEqual(expected,res, $"Description Is legal test failed : {exception}");
        }

        //the prpose of this test is to see thet the illegal Description are failing and an argumentException is trowne
        [Test]
        [TestCase("Lpppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppp")]
        public void DescriptionIsLegalTestFalse(String description)
        {
            Assert.Throws<ArgumentException>(() => TaskTest.DescriptionIsLegal(description));
        }


        //the prpose of this test is to see thet the new DueTime is pass the DueTimeIsLegal and update the old dueTime
        [Test]
        public void UpdateDueTimeTest()
        {
            DateTime time2 = new DateTime(2029, 1, 1);
            taskMock.Setup(m => m.DueTimeIsLegal(time2)).Returns(true);
            taskMock.Setup(m => m.IsAssignee(assignee)).Returns(true);
            dalMock.Setup(m => m.UpdateDueDate(1, 1, time2));
            //action
            TaskTest.SetDueTime(time2, assignee);
            //expected
            DateTime temp = TaskTest.dueTime;
            //Assert
            Assert.AreEqual(time2, temp);
        }
    }
}