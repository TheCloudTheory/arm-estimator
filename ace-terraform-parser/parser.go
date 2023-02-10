package main

import "C"

import (
	"context"
	"encoding/json"
	"fmt"
	"log"
	"os"
	"os/exec"

	"github.com/hashicorp/terraform-exec/tfexec"
)

//export GenerateParsedPlan
func GenerateParsedPlan(workingDir string, planFile string) {
	logFile, err := os.OpenFile("ace-terraform-parser.log", os.O_APPEND|os.O_CREATE|os.O_WRONLY, 0644)
	if err != nil {
		log.Fatalf("Error opening parser log file: %s", err)
	}

	path, err := exec.LookPath("terraform")
	if err != nil {
		logFile.WriteString(fmt.Sprintf("error looking for Terraform executable: %s", err))
		return
	}

	tf, err := tfexec.NewTerraform(workingDir, path)
	if err != nil {
		logFile.WriteString(fmt.Sprintf("error running NewTerraform: %s", err))
		return
	}

	plan, err := tf.ShowPlanFile(context.Background(), planFile)
	if err != nil {
		logFile.WriteString(fmt.Sprintf("error running Show: %s", err))
		return
	}

	ret, err := json.Marshal(plan)
	if err != nil {
		logFile.WriteString(fmt.Sprintf("error marshalling json: %s", err))
		return
	}

	os.WriteFile("ace-terraform-parsed-plan.json", ret, 0644)
}

func main() {
}
