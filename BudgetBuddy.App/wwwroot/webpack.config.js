const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
    entry: {
        'components/modal': './scripts/components/modal.ts',
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
