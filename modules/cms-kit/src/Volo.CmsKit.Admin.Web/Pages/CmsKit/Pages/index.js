$(function () {
    var l = abp.localization.getResource("CmsKit");

    var pagesService = volo.cmsKit.admin.pages.pageAdmin;

    var getFilter = function () {
        return {
            filter: $('#CmsKitPagesWrapper input.page-search-filter-text').val()
        };
    };

    var _dataTable = $("#PagesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollCollapse: true,
        scrollX: true,
        ordering: true,
        order: [[4, "desc"]],
        ajax: abp.libs.datatables.createAjax(pagesService.getList, getFilter),
        columnDefs: [
            {
                title: l("Details"),
                targets: 0,
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.Pages.Update'),
                            action: function (data) {
                                location.href = 'Pages/Update/' + data.record.id;
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.Pages.Delete'),
                            confirmMessage: function (data) {
                                return l("PageDeletionConfirmationMessage")
                            },
                            action: function (data) {
                                pagesService
                                    .delete(data.record.id)
                                    .then(function () {
                                        _dataTable.ajax.reloadEx();
                                        abp.notify.success(l('DeletedSuccessfully'));
                                    });
                            }
                        },
                        {
                            text: l('SetAsHomePage'),
                            visible: abp.auth.isGranted('CmsKit.Pages.SetAsHomePage'),
                            action: function (data) {
                                pagesService
                                    .setAsHomePage(data.record.id)
                                    .then(function () {

                                        _dataTable.ajax.reloadEx();
                                        data.record.isHomePage ?
                                            abp.notify.warn(l('RemovedSettingAsHomePage'))
                                            : abp.notify.success(l('CompletedSettingAsHomePage'));
                                    });
                            }
                        }
                    ]
                }
            },
            {
                title: l("Title"),
                orderable: true,
                data: "title"
            },
            {
                title: l("Slug"),
                orderable: true,
                data: "slug"
            },
            {
                title: l("IsHomePage"),
                orderable: true,
                data: "isHomePage"
            },
            {
                title: l("CreationTime"),
                orderable: true,
                data: 'creationTime',
                dataFormat: "datetime"
            },
            {
                title: l("LastModificationTime"),
                orderable: true,
                data: 'lastModificationTime',
                dataFormat: "datetime"
            }
        ]
    }));

    $('#CmsKitPagesWrapper form.page-search-form').submit(function (e) {
        e.preventDefault();
        _dataTable.ajax.reloadEx();
    });

    $('#AbpContentToolbar button[name=CreatePage]').on('click', function (e) {
        e.preventDefault();
        window.location.href = "Pages/Create"
    });
});