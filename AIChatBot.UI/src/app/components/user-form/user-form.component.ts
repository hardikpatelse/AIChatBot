import { Component, EventEmitter, OnInit, Output } from '@angular/core'
import { UserService } from '../../services/user.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { User } from '../../entities/user'

@Component({
    selector: 'app-user-form',
    templateUrl: './user-form.component.html',
    styleUrls: ['./user-form.component.css'],
    standalone: false
})
export class UserFormComponent implements OnInit {
    user!: User
    userForm!: FormGroup
    buttonText: string = 'Register'
    @Output() userSubmitted = new EventEmitter<User>();

    constructor(private fb: FormBuilder, private userService: UserService) { }
    ngOnInit(): void {
        this.userForm = this.fb.group({
            name: ['', Validators.required],
            email: ['', Validators.required]
        })
    }

    onSubmit(form: any) {
        // this.submitted = true
        if (this.userForm.valid) {
            if (this.user) {
                this.userSubmitted.emit(this.user)
            } else {
                const { name, email } = this.userForm.value
                this.userService.registerUser(name, email).subscribe({
                    next: (user) => {
                        this.user = user
                        this.userSubmitted.emit(this.user)
                        // You can emit an event or handle the user registration success here
                        console.log('User registered successfully:', user)
                    },
                    error: (error) => {
                        console.error('Error registering user:', error)
                        alert('Failed to register user. Please try again.')
                    }
                })
            }
        } else if (!this.userForm.controls['email'].valid) {
            alert('Enter your Email!')
        } else if (!this.userForm.controls['name'].valid) {
            alert('Enter your Name!')
        }
    }

    onEmailBlur(event: any) {
        this.userService.getUserByEmail(event.target.value).subscribe({
            next: (user) => {
                if (user) {
                    this.userForm.patchValue({ name: user.name })
                    this.user = user
                    this.buttonText = 'Start'
                }
            },
            error: (error) => {
                console.log(error)
            }
        })
    }
}
