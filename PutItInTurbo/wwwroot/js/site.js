// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
document.addEventListener("turbolinks:request-end", function(e) {
    console.log("request-end");
    console.log(e);
});

document.addEventListener("turbolinks:visit", function(e) {
    console.log(`visit`);
    console.log(e);
});