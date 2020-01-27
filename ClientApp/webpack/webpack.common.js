const Path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
  entry: {
    app: Path.resolve(__dirname, '../src/app.js')
  },
  output: {
    path: Path.join(__dirname, '../../App/wwwroot/'),
    filename: 'js/app.js'
  },
  optimization: {
    splitChunks: {
      chunks: 'all',
      name: false
    }
  },
  plugins: [
    new CleanWebpackPlugin(['asset'], { root: Path.resolve(__dirname, '../../App/wwwroot') })
    // new CopyWebpackPlugin([
    //   { from: Path.resolve(__dirname, '../asset/css'), to: 'css' }
    // ])
    // new HtmlWebpackPlugin({
    //   template: Path.resolve(__dirname, '../src/www/index.html')
    // })
  ],
  resolve: {
    alias: {
      '~': Path.resolve(__dirname, '../src')
    }
  },
  module: {
    rules: [
      {
        test: /\.mjs$/,
        include: /node_modules/,
        type: 'javascript/auto'
      },
      {
        test: /\.(ico|jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2)(\?.*)?$/,
        use: {
          loader: 'file-loader',
          options: {
            name: '[path][name].[ext]'
          }
        }
      },
    ]
  }
};