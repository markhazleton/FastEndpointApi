namespace FastEndpointApi.endpoints
{
    /// <summary>
    /// Represents a hypermedia link for a resource.
    /// </summary>
    public class LinkResource
    {
        /// <summary>
        /// Gets or sets the relationship name for the link.
        /// </summary>
        public string Rel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target URL for the link.
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTTP method associated with the link.
        /// </summary>
        public string Method { get; set; } = string.Empty;
    }
}