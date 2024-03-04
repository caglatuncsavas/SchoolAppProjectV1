import { Component, ElementRef, ViewChild, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, NgModel } from '@angular/forms';

import { ClassRoomModel } from '../../Models/class-room.model';
import { StudentModel } from '../../Models/student.model';
import { StudentPipe } from '../pipes/student.pipe';
import { HttpService } from '../../services/http.service';
import { FormValidateDirective } from 'form-validate-angular';
import { SwalService } from '../../services/swal.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, StudentPipe,FormValidateDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  classRooms: ClassRoomModel[] = [];
  students: StudentModel[] = [];

  @ViewChild("studentAddModalCloseBtn") studentAddModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;

  addStudentModel:StudentModel = new StudentModel();
  updateStudentModel:StudentModel = new StudentModel();

  selectedRoomId: string = "";
  search: string = "";


  constructor(
    private http: HttpService,
    private swal: SwalService) {
    this.getAllClassRooms();
  }

  getAllClassRooms() {
    this.http.get("ClassRooms/GetAll", (res) => {
      this.classRooms = res;

      if (this.classRooms.length > 0) {
        this.getAllStudentsByClassRoomId(this.classRooms[0].id);
        //this.selectedRoomId = this.classRooms[0].id;
      }
    });
  }

  getAllStudentsByClassRoomId(roomId: string) {
    this.selectedRoomId = roomId;
    this.http.get("Students/GetAllByClassRoomId?classRoomId=" + this.selectedRoomId, res => {
      this.students = res

      this.students = this.students.map((val) => {
        const identityNumberPart1 = val.identityNumber.substring(0, 2);
        const identityNumberPart2 = val.identityNumber.substring(val.identityNumber.length - 6, 3);

        const newHashedIdentityNumber = identityNumberPart1 + "******" + identityNumberPart2;

        val.identityNumber = newHashedIdentityNumber;

        return val;
      })

    });
  }

  createStudent(form:NgForm){
    if(form.valid){
      if(this.addStudentModel.classRoomId === "0"){
        alert("You must select a class room");
        return;
      }
      this.http.post("Students/Create",this.addStudentModel, (res)=>{
        console.log(res);
        this.studentAddModalCloseBtn?.nativeElement.click();
        this.swal.callToast(res.message);
        this.getAllStudentsByClassRoomId(this.addStudentModel.classRoomId);
      })
    }
  }

  clearAddStudentModel(){
    this.addStudentModel = new StudentModel();
    const inputs = document.querySelectorAll(".form-control.is-invalid");
    for(let i in inputs){
      inputs[i].classList.remove("is-invalid");
    }
  }
}
