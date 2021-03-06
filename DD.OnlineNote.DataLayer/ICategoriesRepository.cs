﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD.OnlineNote.Model;


namespace DD.OnlineNote.DataLayer
{
    public interface ICategoriesRepository
    {
        Category Create(Guid userId, string name);
        void Delete(Guid categoriesId);
        IEnumerable<Category> GetUserCategories(Guid userId);
        Category Update(Category ChangedCategory);
        //Category Get(Guid categoriesId);
    }

}
