$('.cart-item-delete').on('click', function (e) {
    e.preventDefault()
    e.stopPropagation()
    const element = $(this)
    const cartId = element.attr('id')
    const url = element.attr('href')

    
    $.ajax({
        url,
        data: {
            cartId
        },
        success: function (data) {

            //update total
            const priceSub = Number($('#'+cartId).parent().siblings(".item-total").text().replace(/[^0-9.-]+/g, ""))
            let total = Number($('.cart-total').text().replace(/[^0-9.-]+/g, ""))
            total = total - priceSub
            total = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(total)
            $('.cart-total').text(total)

            //update item count
            let itemCount = $('.item-count').text()
            itemCount--
            $('.item-count').text(itemCount)
            $('.item-count-unit').text(itemCount > 1 ? "items" : "item");

            element.closest("tr").remove();
            toastr.success('Delete item successfully')
        },
        error: function (err) {
            toastr.error("Delete item failed")
        }
    })
})

$('.cart-form').on('submit', function () {

})