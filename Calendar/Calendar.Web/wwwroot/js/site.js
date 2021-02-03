function confirmDelete(){
    DayPilot.Modal.confirm(`Do you want to delete this reservation?`, { theme: "modal_rounded" }).then(function (args) {
        if (args.result) {
            return true;
        }

        return false;
    });
}