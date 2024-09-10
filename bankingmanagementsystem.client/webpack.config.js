const path = require('path');

module.exports = {
  mode: 'development',
  entry: '.../src/index.js', // Adjust if your entry file is different
  output: {
    filename: 'main.js',
    path: path.resolve(__dirname, 'dist'), // Output directory
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: 'babel-loader', // Make sure you have babel-loader installed if you're using it
      },
    ],
  },
};
