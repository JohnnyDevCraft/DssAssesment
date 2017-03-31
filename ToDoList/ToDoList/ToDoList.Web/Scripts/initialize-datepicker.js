if (typeof shortDatePattern === "undefined" || shortDatePattern === null) {
	shortDatePattern = "yyyy/mm/dd";
}
$(document).ready(function () {
	$(".datepicker").datepicker({
		format: shortDatePattern,
		multidate: false,
		todayHighlight: true
	});
	return;
});