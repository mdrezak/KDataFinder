const url = 'https://senf.ir/AdvancedSearch';
const formData = new FormData();
var e = $("#form1").serializeArray();
e.forEach(x => formData.append(x.name, x.value));
// Object.keys(formJson).forEach(x => formData.append(x,formJson[x]));
fetch(url, {
    method: 'POST',
    headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
    },
    body: formData
})
    .then(response => response.text())
    .then(data => {
        var t = document.createElement('template');
        t.innerHTML = data;
        debugger;
    })
    .catch((error) => {
        console.error('Error:', error);
    });
