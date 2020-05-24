var ds = document.getElementsByClassName('EvETime');

for (var i = 0; i < ds.length; i++) {
	var dt = document.getElementById(i);
	dt.innerHTML = LocalTime(ds[i].innerText);
};

function LocalTime(EvETime) {
	var pde = new Date();
	pde = Date.parse(EvETime);
	var pdt = new Date(pde);
	var dt = new Date();
	dt.setFullYear(pdt.getFullYear());
	dt.setMonth(pdt.getMonth());
	dt.setDate(pdt.getDate());
	dt.setHours(pdt.getHours(), pdt.getMinutes(), pdt.getSeconds());
	dt.setMinutes(dt.getMinutes() + (dt.getTimezoneOffset() * -1))
	return dt.toLocaleString("sv-SE", {
		hour12: false,
		year: "numeric",
		month: "2-digit",
		day: "2-digit",
		hour: "2-digit",
		minute: "2-digit",
		second: "2-digit"
	});
}