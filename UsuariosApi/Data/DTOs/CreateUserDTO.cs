﻿using System.ComponentModel.DataAnnotations;

namespace UsuariosApi.Data.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set;}

        [Required]
        public string Email { get; set; }
    }
}
