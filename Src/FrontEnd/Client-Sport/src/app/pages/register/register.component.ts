import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule
  ]
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const formValue = this.registerForm.value;


      const userData = {
        userName: formValue.userName,
        email: formValue.email,
        password: formValue.password
      };

      this.authService.register(userData).subscribe({
        next: (res) => {
          console.log('ثبت ‌نام موفق:', res);
          localStorage.setItem('token', res.token); // ✅ ذخیره توکن در localStorage
console.log(res.token);
          alert('ثبت‌ نام با موفقیت انجام شد!');
        },
        error: (err) => {
          console.error('خطا در ثبت‌ نام:', err);
          alert('ثبت‌ نام با خطا مواجه شد.');
        }
      });
    }
  }
}
