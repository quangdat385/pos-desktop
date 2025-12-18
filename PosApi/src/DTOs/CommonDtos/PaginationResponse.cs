namespace PosApi.DTOs
{
    using System.Collections.Generic;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Text.Json.Serialization;
    public class PaginationResponse<T>
    {
        [DefaultValue(1)]
        [JsonPropertyName("current_page")]
        public int CurrentPage
        {
            get; set;
        }
        [DefaultValue(50)]
        [JsonPropertyName("page_size")]
        public int PageSize
        {
            get; set;
        }
        [JsonPropertyName("total_pages")]
        public int TotalPages
        {
            get; set;
        }
        [JsonPropertyName("total_items")]
        public int TotalItems
        {
            get; set;
        }
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new List<T>();

        [JsonPropertyName("categories")]
        [DefaultValue(null)]
        public List<string>? Categories
        {
            get; set;
        } = null;
    }
}
