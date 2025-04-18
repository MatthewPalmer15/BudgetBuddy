/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: 'class',
    content: ['../Components/**/*.{razor,cshtml}', './index.html'],
    theme: {},
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
        require('@tailwindcss/aspect-ratio'),
        require('preline/plugin'),
    ],
}

