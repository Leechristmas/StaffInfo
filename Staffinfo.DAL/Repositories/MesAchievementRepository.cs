using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Repositories
{
    /// <summary>
    /// Repository for MesAchievements
    /// </summary>
    public class MesAchievementRepository: IRepository<MesAchievement>
    {
        private readonly StaffContext _staffContext;

        public MesAchievementRepository(StaffContext staffContext)
        {
            _staffContext = staffContext;
        }

        public IEnumerable<MesAchievement> Select()
        {
            return _staffContext.MesAchievements;
        }

        public MesAchievement Select(int id)
        {
            return _staffContext.MesAchievements.Find(id);
        }

        public IEnumerable<MesAchievement> Find(Func<MesAchievement, bool> predicate)
        {
            return _staffContext.MesAchievements.Where(predicate).ToList();
        }

        public void Create(MesAchievement item)
        {
            _staffContext.MesAchievements.Add(item);
        }

        public void Update(MesAchievement item)
        {
            _staffContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            MesAchievement mesAchievement = _staffContext.MesAchievements.Find(id);

            if (mesAchievement != null)
                _staffContext.MesAchievements.Remove(mesAchievement);
        }
    }
}