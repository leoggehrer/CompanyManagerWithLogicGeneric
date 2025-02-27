﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyManager.Logic.Entities
{
    /// <summary>
    /// Represents a customer entity.
    /// </summary>
    [Table("Customers")]
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class Customer : EntityObject, Common.Contracts.ICustomer
    {
        #region properties
        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email of the customer.
        /// </summary>
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        #endregion properties

        #region navigation properties
        /// <summary>
        /// Gets or sets the associated company.
        /// </summary>
        public Common.Contracts.ICompany? Company { get; set; }
        #endregion navigation properties

        #region methods
        /// <summary>
        /// Returns a string representation of the customer.
        /// </summary>
        /// <returns>A string representation of the customer.</returns>
        public override string ToString()
        {
            return $"Customer: {Name}";
        }
        #endregion methods
    }
}
