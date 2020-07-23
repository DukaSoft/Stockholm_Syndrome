

var EveTimes = document.getElementsByClassName('EvETime');


// Update countdown time
function updateCountdown() {
    var currentTime = new Date();

    for (var i = 0; i < EveTimes.length; i++) {
        var cd = document.getElementById(i + " cd");
        if (cd.innerHTML == "Expired!") {
            continue;
		}

        var eveTime = Date.parse(EveTimes[i].innerText);

        // Calculate difference
        const diff = eveTime - currentTime - (currentTime.getTimezoneOffset() * 60 * 1000);

        const d = Math.floor(diff / 1000 / 60 / 60 / 24);
        const h = Math.floor(diff / 1000 / 60 / 60) % 24;
        const m = Math.floor(diff / 1000 / 60) % 60;
        const s = Math.floor(diff / 1000) % 60;

        // Add values to DOM
        var days = d;
        var hours = h < 10 ? '0' + h : h;
        var minutes = m < 10 ? '0' + m : m;
        var seconds = s < 10 ? '0' + s : s;

        var countdown = "";
        if (days > 1) {
            countdown = days + " Days " + hours + ":" + minutes + ":" + seconds;
		} else if (days == 1) {
            countdown = days + " Day " + hours + ":" + minutes + ":" + seconds;
        } else if (days == 0) {
            countdown = hours + ":" + minutes + ":" + seconds;
        }
        if (countdown == "") {
            countdown = "Expired!"
		}
        cd.innerHTML = countdown;
    };
}

// Run every second
setInterval(updateCountdown, 1000);
