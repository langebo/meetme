const generateInitials = name => {
  if (!name) return '';
  var names = name.split(' ');
  var initials = names[0].substring(0, 1).toUpperCase();
  if (names.length > 1)
    initials += names[names.length - 1].substring(0, 1).toUpperCase();

  return initials;
}

export default generateInitials;
