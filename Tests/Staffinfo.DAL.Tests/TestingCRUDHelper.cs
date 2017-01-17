using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Models.Common;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    public static class TestingCRUDHelper
    {
        private static int INT_DEFAULT_FOR_TESTING = 999;
        private static long LONG_DEFAULT_FOR_TESTING = 999;
        private static string STRING_DEFAULT_FOR_TESTING = "t";
        private static DateTime DATETIME_DEFAULT_FOR_TESTING = DateTime.Now;
        
        /// <summary>
        /// Generic method for testing of deleting items from DB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task Delete_ShouldDeleteSpecifiedItemFromDB<T>(IRepository<T> repository, int id) where T : Entity
        {
            await repository.Delete(id);
            await repository.SaveAsync();
            var deleted = await repository.SelectAsync(id);

            Assert.IsNull(deleted, "the item has not been deleted!");
        }

        /// <summary>
        /// Generic method for testing of getting items from DB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetItem_ShouldReturnAnItemById<T>(IRepository<T> repository, int id) where T : Entity
        {
            T item = repository.SelectAsync(id).Result;

            Assert.IsNotNull(item, "returned item is \"NULL\"");

            return item;
        }

        /// <summary>
        /// Generic method for testing of getting all items from DB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        public static void GetAllItems_ShouldReturnAllItemsFromDB<T>(IRepository<T> repository) where T: Entity
        {
            List<T> items = null;

            items = repository.SelectAsync().Result.ToList();

            Assert.IsNotNull(items, "The list of item is \"NULL\"");
            Assert.IsTrue(items.Count > 0, "No items have been returned.");
        }

        //public static async Task Update_ShouldUpdateSpecifiedItemFromDB<T>(IRepository<T> repository, T item) where T: Entity
        //{
        //    FieldInfo[] fieldInfos;
        //    fieldInfos = item.GetType().GetFields(BindingFlags.NonPublic |
        //                                     BindingFlags.Instance);

        //    //item.Title = "test_title";
        //    //item.Description = "testing of updating";

        //    //_repository.DisciplineItemRepository.Update(item);
        //    //await _repository.DisciplineItemRepository.SaveAsync();

        //    //DisciplineItem updated = _repository.DisciplineItemRepository.SelectAsync(item.Id).Result;

        //    //Assert.AreEqual(updated.Title, item.Title, "The \"title\" has not been updated!");
        //    //Assert.AreEqual(updated.Description, item.Description, "The \"description\" has not been updated!");

        //    foreach (var field in fieldInfos)
        //    {
        //        if (field.FieldType == typeof(int) || field.FieldType == typeof(int?))
        //            field.SetValue(item, INT_DEFAULT_FOR_TESTING);
        //        else if (field.FieldType == typeof(long) || field.FieldType == typeof(long?))
        //            field.SetValue(item, LONG_DEFAULT_FOR_TESTING);
        //        else if (field.FieldType == typeof(string))
        //            field.SetValue(item, STRING_DEFAULT_FOR_TESTING);
        //        else if (field.FieldType == typeof(DateTime) || field.FieldType == typeof(DateTime?))
        //            field.SetValue(item, DATETIME_DEFAULT_FOR_TESTING);
        //    }

        //    repository.Update(item);
        //    await repository.SaveAsync();

        //    T updated = repository.SelectAsync(item.Id).Result;

        //    try
        //    {
        //        foreach (var field in fieldInfos)
        //        {
        //            if (field.FieldType == typeof(int) || field.FieldType == typeof(int?))
        //                Assert.AreEqual(field.GetValue(item), INT_DEFAULT_FOR_TESTING);
        //            else if (field.FieldType == typeof(long) || field.FieldType == typeof(long?))
        //                Assert.AreEqual(field.GetValue(item), LONG_DEFAULT_FOR_TESTING);
        //            else if (field.FieldType == typeof(string))
        //                Assert.AreEqual(field.GetValue(item), STRING_DEFAULT_FOR_TESTING);
        //            else if (field.FieldType == typeof(DateTime) || field.FieldType == typeof(DateTime?))
        //                Assert.AreEqual(field.GetValue(item), DATETIME_DEFAULT_FOR_TESTING);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }

        //    int y = 0;

        //}
    }
}