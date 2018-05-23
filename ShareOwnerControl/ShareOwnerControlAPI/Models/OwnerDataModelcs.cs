﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ShareOwnerControlAPI.Models
{
    public class OwnerDataModel
    {
        public OwnerDataModel()
        {
        }

        [Key]
        public int Id { get; set; }
        public Guid ShareOwner { get; set; }
    }
}