function GridView_rowClick(grid, allowMultipleSelect, doPostback)
{
    if (!allowMultipleSelect)
        grid._selectAllRowsOnPage(false);
    grid.SelectRow(eventArgs.visibleIndex, true);
}