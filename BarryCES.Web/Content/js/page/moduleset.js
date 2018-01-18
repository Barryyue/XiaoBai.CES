(function () {
    $("#ParentName").bsSuggest({
        allowNoKeyword: true,
        multiWord: true,
        showHeader: true,
        effectiveFieldsAlias: { Id: "主键", ModuleName: "名称", TypeName: "类型", Url: "URL地址" },
        effectiveFields: ["Id", "ModuleName", "TypeName", "Url"],
        getDataMethod: "url",
        url: "/ModuleSet/GetListWithKeywords?keywords=",
        idField: "Id",
        keyField: "ModuleName"
    }).on('onSetSelectValue', function (e, data) {
        $("#ParentId").val(data.id);
    }).on('onUnsetSelectValue', function () {
        $("#ParentId").val("");
    });
})();