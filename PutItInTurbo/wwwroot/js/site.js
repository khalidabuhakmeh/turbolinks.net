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

$(document).on("submit", "#form", function (e) {
   e.preventDefault();
   let $form = $(this);
   
   $.ajax({
       type: $form.attr('method'),
       url: $form.attr('action'),
       data: $form.serialize(),
       success: function (result) {
           console.log(result);
       } 
   });
});