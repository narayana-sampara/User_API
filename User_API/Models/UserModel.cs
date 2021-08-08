using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace User_API.Models
{


    [Table("tbl_userinfo")]
    public class UserModel
    {
        
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [Column()]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int Age { get; set; }
    }
    public class ResponseResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
