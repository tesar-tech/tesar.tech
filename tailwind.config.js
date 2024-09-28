const colors = require('tailwindcss/colors');
module.exports = {
    content:
        [
            './Components/**/*.razor',
            './Content/**/*.md',
        ],
    theme: {
        extend: {
            fontFamily: {
                'tomorrow': ['Tomorrow', 'sans'],
            },
            colors: {

                primary: colors.violet

            },
        },},
    plugins: [require('@tailwindcss/typography')],
}


