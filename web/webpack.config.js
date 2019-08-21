module: {
  rules: [
    {
      test: /\.js$/,
      exclude: /node_modules\/(?!(@react-simply)\/).*/,
      loader: 'babel-loader',
    },
  ];
}
