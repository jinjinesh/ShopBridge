module.exports = {
    root: true,
    env: {
        node: true
    },
    extends: [
        'plugin:vue/strongly-recommended',
        'standard'
    ],
    parserOptions: {
        parser: 'babel-eslint'
    },
    rules: {
        'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        // allow paren-less arrow functions
        'arrow-parens': 0,
        // allow async-await
        'generator-star-spacing': 0,
        // enforce semi-colons
        semi: [2, 'always'],
        // 4-space indention
        indent: ['error', 4]
    }
};
