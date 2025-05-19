// This script injects a link to /person-demo.html at the top of the Swagger UI page.
(function() {
  function addDemoLink() {
    var container = document.querySelector('.swagger-ui .topbar');
    if (!container) return;
    var link = document.createElement('a');
    link.href = '/person-demo.html';
    link.textContent = 'Person Demo UI';
    link.target = '_blank';
    link.style.marginLeft = '2em';
    link.style.fontWeight = 'bold';
    link.style.color = '#007bff';
    container.appendChild(link);
  }
  // Wait for Swagger UI to load
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', addDemoLink);
  } else {
    setTimeout(addDemoLink, 500);
  }
})();
