﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VipCon.Models
{
    public class Prospect
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "É obrigatório informar um telefone")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Formato de telefone inválido")]        
        public string Telefone { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "É obrigatório informar a modalidade do consórcio")]
        public string ModalidadeConsorcio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy HH:mm}")]
        public DateTime DataSimulacao { get; set; }
    }
}
