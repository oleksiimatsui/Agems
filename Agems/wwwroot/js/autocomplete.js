options = [];

// create an array of select options for a lookup
$('.custom-select option').each(function (idx) {
    options.push({ id: $(this).val(), text: $(this).text() });
});


$("select").select2({
    tags: "true",
    placeholder: "Select an option",
    allowClear: true,
    width: '100%',
    createTag: function (params) {
        var term = $.trim(params.term);

        if (term === '') {
            return null;
        }

        // check whether the term matches an id
        var search = $.grep(options, function (n, i) {
            return (n.id === term || n.text === term); // check against id and text
        });

        // if a match is found replace the term with the options' text
        if (search.length)
            term = search[0].text;
        else
            return null; // didn't match id or text value so don't add it to selection

        return {
            id: term,
            text: term,
            value: true // add additional parameters
        }
    }
});

$('select').on('select2:select', function (evt) {
    //console.log(evt);
    //return false;
});