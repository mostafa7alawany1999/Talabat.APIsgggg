﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.EmpolyeeSpec
{
    public class EmployeeWithDepartmentSpecifications : BaseSpecifications<Employee>
    {

        public EmployeeWithDepartmentSpecifications() : base()
        {
            Includes.Add(E => E.Department);
        }

        public EmployeeWithDepartmentSpecifications(int id) : base(E=>E.Id==id)
        {
            Includes.Add(E => E.Department);
        }
    }
}
