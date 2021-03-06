﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VidlyMovieRentalApp.Models;

namespace VidlyMovieRentalApp.Dtos
{
    public class CustomerDto
    {
       // [Min18YearsIfAMember]
        public DateTime? Birthdate { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Customer Name")]
        [StringLength(255)]

        public string Name { get; set; }

        public bool IsSubscribedToNewsLetter { get; set; }

        public MembershipTypeDto MembershipType { get; set; }
        public byte MembershipTypeId { get; set; }

    }
}