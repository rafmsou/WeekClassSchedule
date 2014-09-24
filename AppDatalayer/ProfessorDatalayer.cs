﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WeekClassSchedule.AppData;
using WeekClassSchedule.Extensions;

namespace WeekClassSchedule.AppDatalayer
{
    public class ProfessorDatalayer
    {
        private WeekClassScheduleEntities _entitiesDb;
        private List<Professor> _professorList;

        public ProfessorDatalayer()
        {
           _entitiesDb = new WeekClassScheduleEntities();
        }

        public Professor GetById(int id)
        {
            return _entitiesDb.Professor.Single(p => p.Id == id);
        }

        public List<Professor> GetProfessorList()
        {
            if (_professorList == null)
                _professorList = _entitiesDb.Professor.OrderBy(i => i.Name).ToList();

            return _professorList;
        }

        public async Task<List<Professor>> GetProfessorListAsync()
        {
            return await _entitiesDb.Professor.OrderBy(i => i.Name).ToListAsync();
        }

        public void SaveOrUpdate(Professor professorToSave)
        {
            if (professorToSave.Id == 0)
                _entitiesDb.Professor.Add(professorToSave);
            else
            {
                var professor = this.GetById(professorToSave.Id);
                professor = professorToSave;
            }

            _entitiesDb.SaveChanges();
        }

        public void Remove(int professorId)
        {
            var professor = this.GetById(professorId);
            _entitiesDb.Professor.Remove(professor);
            _entitiesDb.SaveChanges();
        }

        public List<Professor> GetForScheduling(DayOfWeek dayOfWeek, int classNumber)
        { 
            return _entitiesDb.Professor
                        .Where(p => p.AttendanceRules.Any(rule => rule.ClassNumber == classNumber && rule.DayOfWeek == (int)dayOfWeek)
                        && !p.HasSchedule(dayOfWeek, classNumber)).ToList();  
        }
    }
}
