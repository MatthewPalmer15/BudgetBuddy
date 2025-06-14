const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const TerserPlugin = require('terser-webpack-plugin');
const WebpackObfuscator = require('webpack-obfuscator');

module.exports = {
    entry: {
        'components/modal': './scripts/components/modal.ts',
        'pages/transactions/edit': './scripts/pages/transactions/edit.ts',
        'pages/home': './scripts/pages/home.ts',
        'main': './scripts/main.ts',
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'dist'),
        clean: true,
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader',
                    'postcss-loader',
                    'sass-loader',
                ],
            },
            {
                test: /\.(ttf|woff2?|eot|otf)$/,
                type: 'asset/resource',
                generator: {
                    filename: 'fonts/[name][ext]'
                }
            }
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: '[name].css',
        }),
        new CopyWebpackPlugin({
            patterns: [
                {
                    from: path.resolve(__dirname, 'scripts/libs/vanta.waves.min.js'),
                    to: path.resolve(__dirname, 'dist/libs/vanta.waves.min.js')
                },
                {
                    from: path.resolve(__dirname, 'scripts/libs/three.min.js'),
                    to: path.resolve(__dirname, 'dist/libs/three.min.js')
                }
            ]
        }),
        // new WebpackObfuscator(
        //     {
        //         rotateStringArray: true,
        //         stringArray: true,
        //         stringArrayThreshold: 0.75,
        //         compact: true,
        //         controlFlowFlattening: true,
        //         deadCodeInjection: true,
        //         debugProtection: true,
        //         selfDefending: true,
        //     },
        //     ['**/libs/*.js']
        // ),
    ],
    mode: 'production',
    optimization: {
        minimize: true,
        minimizer: [
            new TerserPlugin({
                extractComments: false, // disables .LICENSE.txt files
            }),
        ],
    }
    //optimization: {
    //    splitChunks: {
    //        chunks: 'all',
    //        minSize: 0,   
    //        cacheGroups: {
    //            defaultVendors: {
    //                test: /[\\/]node_modules[\\/]/,
    //                name: 'vendors',
    //                chunks: 'all',
    //            },
    //            common: {
    //                name: 'common',
    //                minChunks: 2,
    //                chunks: 'all',
    //                reuseExistingChunk: true,
    //            },
    //        },
    //    },
    //}
};
