# Securo

This Cryptography Project aimed at combining Steganography and Cryptography techniques and form a hybrid encryption method for message/data sharing. Developed an algorithm by customizing and integrating AES and LSB techniques to encrypt the given data and then the cypher data is hidden inside the cover image. This tool provides a novel, secure way to transfer confidential data.

Using the AES and LSB techniques to encrypt and hide the information into a given image, we can successfully increase the security of the vital information by certain levels. This method will also help the users to reduce the risk of information eavesdropping and leakage of information by intruders. Hence, the proposed system provides enhanced security. Also the proposed system supports a wider range of image types; which in turn helps a wider range of users to use this system. The system uses a unique LSB technique which maintains good quality of the image. And also the new system is more user friendly and anyone with the basic computer knowledge can use the system without any difficulties encountered while using the application.

![SECURO](https://user-images.githubusercontent.com/97561730/151731193-0279aa96-38f2-416a-bf92-1181cb55dda2.jpg)
SECURO Welcome Page of Software

![Select the format](https://user-images.githubusercontent.com/97561730/151731225-f44ec6ab-0626-41f1-a765-fe6ae97d0ecc.jpg)
Select the format Window to Navigate desired Encryption/Decryption. Also to change password

![35hmpnz0bDz0rwegl4bwsXjgNN6g5RJapf6OTjcjyzsT640Ts7JIgAn2VeRGWJYt_0NzKigA2hAlK6FStCPDgei0glRaAiIG0ayx3Lr2tPGwX4bTNGFwlk9pMya6](https://user-images.githubusercontent.com/97561730/151731265-b4b8b4f9-9655-476a-b4ff-19c0e12d5aa0.jpg)
Secret Key Text Encryption window after completing Encryption.

![Cover Image](https://user-images.githubusercontent.com/97561730/151731276-4a1daef8-a25a-4a13-8be9-ae7f40a5df90.jpg)
3xz7Sl1IVaVFlnNGE-Vun69QHary3If2KIAHBBGa3SRfghmub-UrNtNy64Tc-USkYWtwqt2uv19fe9s3C_CM2Rk9zIw1LzUwA4kz_DpwmYDYPlJTWgRiN1sorAQE Text Decryption window after the Process.

![Secret Key](https://user-images.githubusercontent.com/97561730/151731285-fe5cac54-5999-4a72-a465-1dba0af77c25.jpg)
35hmpnz0bDz0rwegl4bwsXjgNN6g5RJapf6OTjcjyzsT640Ts7JIgAn2VeRGWJYt_0NzKigA2hAlK6FStCPDgei0glRaAiIG0ayx3Lr2tPGwX4bTNGFwlk9pMya6 Image Encryption window after completing encryption process.

![3xz7Sl1IVaVFlnNGE-Vun69QHary3If2KIAHBBGa3SRfghmub-UrNtNy64Tc-USkYWtwqt2uv19fe9s3C_CM2Rk9zIw1LzUwA4kz_DpwmYDYPlJTWgRiN1sorAQE](https://user-images.githubusercontent.com/97561730/151731297-cc7a4c82-5dee-4e3c-8410-c09efd180371.jpg)
Cover Image Image Decryption window after completion process.

Existing System and its Disadvantages:
* In the existing system, the image or the text is directly hidden in the cover image. Thus, the hacker or the interloper can use the software called “Steg-Analyser” and extract the significant and the substantial bits in the image and transfigure it back into its original form.
* In the existing system, the message is hidden directly in the image. Hence, the message in not encrypted.
* The existing system now only provisions only 16-bit BMP images or other low quality images.
* The system provisions only very few image file types, thus only a limited amount of users can benefit from this system.
* The existing systems may provide more data hiding capacity by manipulating more bits of the given pixel. Here the pixels of image are more polluted and thus the image quality is very low and the noise is very high.
* The existing system stores only in the least significant bit and thus results in very low noise in the image but very less data can be stored in this way.
* The existing systems are very complex to use and the users require minimum knowledge about the existing system.
* The cover image will have more noise while storing large amount of data in the existing system.

Proposed System and its Advantages:
* The proposed system first encrypts the given message using the AES encryption and then it is written onto the cover image. This technique provides higher amount of security. Even higher-level software like “Steg-Analyser” will be of no help to the intruder in this scenario.
* The proposed system will be able to support 24-bit BMP images and also other high quality images.
* The proposed system will be able to support more number of image file types, thus making it available to more number of users.
* The proposed system will be able to store more data and also maintain the quality of the image by using a hybrid approach to the LSB technique.
* We aim to make the proposed system simple so that even a person with the basic computer knowledge can use it.
* We aim to make the proposed system user-friendly and more compact than the existing systems.
