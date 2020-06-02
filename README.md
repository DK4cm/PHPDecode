# PHPDecode
A program to decode the PHP file encode with 微盾

Project Stopped since there is a  BUG that cannot be fixed at this moment.

If there is Chinese word or remark, the decode will not correct, but the source code is correct(It means it cannot decode non ASCII Character).

The bug is due to the special 'magic' function 'strtr' in PHP, if need to fix it, it need to know the inside working of 'strtr' function. 

At this moment, I only create the 'strtr' function by string builder, but it should achive it in more binary way.

If you really need to use these function, I suggested to use the PHP version.
