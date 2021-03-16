window.getDimensions = function() {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

//var fs = require('fs');
//var app = require('electron').remote; 
//var dialog = app.dialog;
//dialog.showSaveDialog((fileName) => {
//    alert(fileName);
//});
//var filepath = "C:/Users/Chi/Desktop/existinfile.txt";// you need to save the filepath when you open the file to update without use the filechooser dialog againg
//var content = "This is the new content of the file";

//fs.writeFile(filepath, content, (err) => {
//    if (err) {
//        alert("An error ocurred updating the file" + err.message);
//        console.log(err);
//        return;
//    }

//    alert("The file has been succesfully saved");
//});